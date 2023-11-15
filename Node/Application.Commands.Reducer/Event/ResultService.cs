using Application.Commands.Interfaces;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;

namespace Application.Commands.Reducer.Event;

public class ResultService : IResultService
{
    private static int iteration = 0;

    private readonly IHostInfo _hostInfo;


    private ISingletonCache<Group> _groupCache;
    private ISingletonCache<ReduceFinishedBool> _groupFinishedCache;

    public ResultService(ISingletonCache<Group> groupCache, ISingletonCache<ReduceFinishedBool> groupFinishedCache, IHostInfo hostInfo)
    {                    
        _groupFinishedCache = groupFinishedCache;
        _groupCache = groupCache;
        _hostInfo = hostInfo;
    }

    public void DisplayResults()
    {
        foreach (var group in _groupCache.Value.Values)
        {
            //string groupId = kvp.Id;

            // todo: this
            //IEnumerable<Space> spaces = kvp.Value.Spaces;

            Console.WriteLine(group);

            //foreach (var space in spaces)
            //{
            //    Console.WriteLine($"  Space ID: {space.Id}, Width: {space.Width}, Price: {space.Price}");
            //}

            //Console.WriteLine();
        }
    }

    public void ReceiveResult(string groupId) 
    {
        _groupFinishedCache.Value.TryAdd(groupId, new ReduceFinishedBool { Value = true, Id = groupId });
    }

    public bool HasFinishedCollectedResults()
    {
        return _groupCache.Value.Values.Count() == _groupFinishedCache.Value.Values.Count();
    }

    public bool HasMoreIterations()
    {
        return iteration < _hostInfo.NbOfIteration;
    }

    public void IncrementIteration()
    {
        Interlocked.Increment(ref iteration);

        _groupFinishedCache.Value.Clear();
    }
}
