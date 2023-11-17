using Application.Commands.Interfaces;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Reducer.Event;

public class ResultService : IResultService
{
    private static int iteration = 0;

    private readonly ILogger<IResultService> _logger;

    private readonly IHostInfo _hostInfo;

    private ISyncStore<string, Group> _groupCache;
    private ISyncStore<string, MapFinishedBool> _spaceResultReceived;
    private ISyncStore<string, ReduceFinishedBool> _groupResultReceived;

    public ResultService(ILogger<IResultService> logger, ISyncStore<string, Group> groupCache, ISyncStore<string, Space> spaceCache, ISyncStore<string, ReduceFinishedBool> groupResultReceived, ISyncStore<string, MapFinishedBool> spaceResultReceived, IHostInfo hostInfo)
    {                    
        _logger = logger;
        _groupCache = groupCache;
        _groupResultReceived = groupResultReceived;
        _spaceResultReceived = spaceResultReceived;
        _hostInfo = hostInfo;
    }

    public void DisplayResults()
    {
        var getGroups = _groupCache.Query(g => g);

        Task.WaitAll(getGroups);

        getGroups.Result.ForEach(group => 
        {
            _logger.LogInformation("");
            _logger.LogInformation("Result:");
            _logger.LogInformation("");
            _logger.LogInformation($"{group}");
            _logger.LogInformation("");
        });
    }

    public void ReceiveResult(string groupId) 
    {
        Task.WaitAll(_groupResultReceived.AddOrUpdate(groupId, new ReduceFinishedBool { IsFinished = true, Id = groupId }));
        Task.WaitAll(_groupResultReceived.SaveChangesAsync());
    }

    public bool HasFinishedCollectedResults()
    {
        var getGroupResultReceived = _groupResultReceived.Query(q => q.Where(s => s.IsFinished));
        var getGroups = _groupCache.Query(g => g);

        Task.WaitAll(getGroupResultReceived, getGroups);

        return getGroupResultReceived.Result.Count() == getGroups.Result.Count();
    }

    public bool HasMoreIterations()
    {
        return iteration < _hostInfo.NbOfIteration;
    }

    public async void IncrementIteration()
    {
        List<Group>? groups = null;
        List<ReduceFinishedBool>? groupsFinished = null;
        List<MapFinishedBool>? spacesFinished = null;

        Task.WaitAll(new Task[] { 
            Task.Run(async () => { groups = await _groupCache.Query(g => g); } ),
            Task.Run(async () => { groupsFinished = await _groupResultReceived.Query(g => g); } ),
            Task.Run(async () => { spacesFinished = await _spaceResultReceived.Query(g => g); } )
        });

        if (groups != null)
        {
            groups.ForEach(g => g.Spaces.Clear()); // second space clear (failsafe). Maybe not needed.
            await _groupCache.AddOrUpdateRange(groups.Select(g => (g.Id, g)));
        }
            

        if (groupsFinished != null)
        {
            groupsFinished.ForEach(g => g.IsFinished = false);
            await _groupResultReceived.AddOrUpdateRange(groupsFinished.Select(g => (g.Id, g)));
        }
           

        if (spacesFinished != null)
        {
            spacesFinished.ForEach(s => s.IsFinished = false);
            await _spaceResultReceived.AddOrUpdateRange(spacesFinished.Select(g => (g.Id, g)));
        }

        Task.WaitAll(new Task[] {
            Task.Run(_groupResultReceived.SaveChangesAsync),
            Task.Run(_spaceResultReceived.SaveChangesAsync),
            Task.Run(_groupCache.SaveChangesAsync)
        });

        Interlocked.Increment(ref iteration);
    }
}
