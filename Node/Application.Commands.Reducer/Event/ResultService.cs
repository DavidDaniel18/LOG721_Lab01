using Application.Commands.Interfaces;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Reducer.Event;

public class ResultService : IResultService
{
    private const double EPSILON = 1.0; 

    private static int _iteration = 1;

    private readonly ILogger<IResultService> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly ISyncStore<string, Group> _groupCache;
    private readonly ISyncStore<string, MapFinishedBool> _spaceResultReceived;
    private readonly ISyncStore<string, ReduceFinishedBool> _groupResultReceived;

    public ResultService(ILogger<IResultService> logger, ISyncStore<string, Group> groupCache, ISyncStore<string, ReduceFinishedBool> groupResultReceived, ISyncStore<string, MapFinishedBool> spaceResultReceived, IHostInfo hostInfo)
    {                    
        _logger = logger;
        _groupCache = groupCache;
        _groupResultReceived = groupResultReceived;
        _spaceResultReceived = spaceResultReceived;
        _hostInfo = hostInfo;
    }

    public async Task DisplayResults()
    {
        var groups = await _groupCache.Query(g => g);

        _logger.LogError("Result:");

        groups.ForEach(group => 
        {
            _logger.LogError($"{group}");
        });
    }

    public async Task ReceiveResult(string groupId, double delta)
    {
        _logger.LogInformation($"ReceiveResult: groupId: {groupId}, delta: {delta}");

        await _groupResultReceived.AddOrUpdate(groupId, new ReduceFinishedBool { IsFinished = true, Id = groupId, Delta = delta });

        await _groupResultReceived.SaveChangesAsync();

        _logger.LogInformation($"ReceiveResult: groupId: {groupId}, delta: {delta}, [SAVED]");
    }

    public async Task<bool> HasFinishedCollectedResults()
    {
        var groupsResultReceivedFinished = await _groupResultReceived.Query(q => q.Where(s => s.IsFinished));

        var getGroups = await _groupCache.Query(g => g);

        _logger.LogInformation($"finished count: {groupsResultReceivedFinished.Count()}, nb of result: {groupsResultReceivedFinished.Count()}, nb of groups: {getGroups.Count()}");

        return groupsResultReceivedFinished.Count().Equals(getGroups.Count());
    }

    public async Task<bool> HasMoreIterations()
    {
        if (_hostInfo.NbOfIteration >= 0) 
            return _iteration < _hostInfo.NbOfIteration;

        var groupFinished = await _groupResultReceived.Query(g => g);

        _logger.LogInformation($"Print Iteration Result Delta:");

        var getGroupResultReceivedDeltaUnderEpsilon = groupFinished.Where(reduceFinishedBool => 
        {
            _logger.LogInformation($"GroupId:{reduceFinishedBool.Id}, Delta:{reduceFinishedBool.Delta}");

            return reduceFinishedBool is { Delta: <= EPSILON };
        });

        var getGroup = await _groupCache.Query(q => q);

        return !getGroup.Count().Equals(getGroupResultReceivedDeltaUnderEpsilon.Count());
    }

    public async Task IncrementIteration()
    {
        var groupsFinished = await _groupResultReceived.Query(g => g);

        groupsFinished.ForEach(reduceFinishedBool => reduceFinishedBool.IsFinished = false);

        await _groupResultReceived.AddOrUpdateRange(groupsFinished.Select(reduceFinishedBool => (reduceFinishedBool.Id, g: reduceFinishedBool)).ToList());

        await _groupResultReceived.SaveChangesAsync();

        var spacesFinished = await _spaceResultReceived.Query(g => g);

        spacesFinished.ForEach(s => s.IsFinished = false);
        
        await _spaceResultReceived.AddOrUpdateRange(spacesFinished.Select(g => (g.Id, g)).ToList());

        await _spaceResultReceived.SaveChangesAsync();

        Interlocked.Increment(ref _iteration);
    }
}
