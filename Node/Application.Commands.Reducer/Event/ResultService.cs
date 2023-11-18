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

    private static int iteration = 1;

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

        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("Result:");

        groups.ForEach(group => 
        {
            _logger.LogInformation($"{group}");
        });

        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
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
        var getGroupResultReceived = await _groupResultReceived.Query(q => q);
        var groupsResultReceivedFinished = getGroupResultReceived.Where(s => s.IsFinished);
        var getGroups = await _groupCache.Query(g => g);

        _logger.LogInformation($"finished count: {groupsResultReceivedFinished.Count()}, nb of result: {getGroupResultReceived.Count()}, nb of groups: {getGroups.Count()}");

        return getGroupResultReceived.Count().Equals(getGroups.Count());
    }

    public async Task<bool> HasMoreIterations()
    {
        if (_hostInfo.NbOfIteration >= 0) 
            return iteration < _hostInfo.NbOfIteration;

        var getGroup = await _groupCache.Query(q => q);
        var getGroupResultReceivedDeltaUnderEpsilon = await _groupResultReceived.Query(q => q.Where(s => s.IsFinished && s.Delta <= EPSILON));
        
        return getGroup.Count().Equals(getGroupResultReceivedDeltaUnderEpsilon.Count());
    }

    public async Task IncrementIteration()
    {
        var groupsFinished = await _groupResultReceived.Query(g => g);

        groupsFinished.ForEach(g => g.IsFinished = false);

        await _groupResultReceived.AddOrUpdateRange(groupsFinished.Select(g => (g.Id, g)).ToList());

        await _groupResultReceived.SaveChangesAsync();

        var spacesFinished = await _spaceResultReceived.Query(g => g);

        spacesFinished.ForEach(s => s.IsFinished = false);
        
        await _spaceResultReceived.AddOrUpdateRange(spacesFinished.Select(g => (g.Id, g)).ToList());

        await _spaceResultReceived.SaveChangesAsync();

        Interlocked.Increment(ref iteration);
    }
}
