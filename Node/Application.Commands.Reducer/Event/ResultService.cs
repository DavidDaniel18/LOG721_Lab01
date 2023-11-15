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

    private ISyncStore<string, Group> _groupCache;
    private ISyncStore<string, MapFinishedBool> _spaceResultReceived;
    private ISyncStore<string, ReduceFinishedBool> _groupResultReceived;

    public ResultService(ISyncStore<string, Group> groupCache, ISyncStore<string, Space> spaceCache, ISyncStore<string, ReduceFinishedBool> groupResultReceived, ISyncStore<string, MapFinishedBool> spaceResultReceived, IHostInfo hostInfo)
    {                    
        _groupCache = groupCache;
        _groupResultReceived = groupResultReceived;
        _spaceResultReceived = spaceResultReceived;
        _hostInfo = hostInfo;
    }

    public void DisplayResults()
    {
        var t1 = _groupCache.Query(q => q.Where(g => true));

        Task.WaitAll(t1);

        foreach (var kvp in t1.Result)
        {
            string groupId = kvp.Id;

            // todo: this
            //IEnumerable<Space> spaces = kvp.Value.Spaces;

            Console.WriteLine(kvp);

            //foreach (var space in spaces)
            //{
            //    Console.WriteLine($"  Space ID: {space.Id}, Width: {space.Width}, Price: {space.Price}");
            //}

            //Console.WriteLine();
        }
    }

    public void ReceiveResult(string groupId) 
    {
        Task.WaitAll(_groupResultReceived.AddOrUpdate(groupId, new ReduceFinishedBool { Value = true, Id = groupId }));
        Task.WaitAll(_groupResultReceived.SaveChangesAsync());
    }

    public bool HasFinishedCollectedResults()
    {
        var t1 = _groupResultReceived.Query(q => q.Where(s => s.Value));
        var t2 = _groupCache.Query(q => q.Where(s => true));

        Task.WaitAll(t1, t2);

        return t1.Result.Count() == t2.Result.Count();
    }

    public bool HasMoreIterations()
    {
        return iteration < _hostInfo.NbOfIteration;
    }

    public void IncrementIteration()
    {
        var t1 = _groupResultReceived.Query(q => q.Where(s => s.Value));
        var t2 = _spaceResultReceived.Query(q => q.Where(s => s.Value));

        t1.Result.ForEach(e => _groupResultReceived.Remove(e.Id));
        t2.Result.ForEach(e => _spaceResultReceived.Remove(e.Id));

        Task.WaitAll(new Task[] {
            Task.Run(_groupResultReceived.SaveChangesAsync),
            Task.Run(_spaceResultReceived.SaveChangesAsync)
        });

        Interlocked.Increment(ref iteration);
    }
}
