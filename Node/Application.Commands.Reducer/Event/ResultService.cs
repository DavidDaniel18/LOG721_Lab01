﻿using Application.Commands.Interfaces;
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

        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("Result:");
        getGroups.Result.ForEach(group => 
        {
            _logger.LogInformation($"{group}");
        });
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
        _logger.LogInformation("");
    }

    public void ReceiveResult(string groupId, double delta) 
    {
        Task.WaitAll(_groupResultReceived.AddOrUpdate(groupId, new ReduceFinishedBool { IsFinished = true, Id = groupId, Delta = delta }));
        Task.WaitAll(_groupResultReceived.SaveChangesAsync());
    }

    public bool HasFinishedCollectedResults()
    {
        var getGroupResultReceived = _groupResultReceived.Query(q => q.Where(s => s.IsFinished));
        var getGroups = _groupCache.Query(g => g);

        Task.WaitAll(getGroupResultReceived, getGroups);

        _logger.LogInformation($"finished count: {getGroupResultReceived.Result.Count()}, nb of groups: {getGroups.Result.Count()}");
        return getGroupResultReceived.Result.Count() == /*getGroups.Result.Count()*/ 1;
    }

    public bool HasMoreIterations()
    {
        if (_hostInfo.NbOfIteration >= 0) 
            return iteration < _hostInfo.NbOfIteration;

        var getGroup = _groupCache.Query(q => q);
        var getGroupResultReceivedDeltaUnderEpsilon = _groupResultReceived.Query(q => q.Where(s => s.IsFinished && s.Delta <= EPSILON));
        Task.WaitAll(getGroup, getGroupResultReceivedDeltaUnderEpsilon);
        
        return getGroup.Result.Count() == getGroupResultReceivedDeltaUnderEpsilon.Result.Count();
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
