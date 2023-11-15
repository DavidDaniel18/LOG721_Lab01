using Application.Commands.Interfaces;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Reducer.Event;

public class ResultService : IResultService
{
    private static int iteration = 0;

    private readonly IHostInfo _hostInfo;

    private Dictionary<string, Group> _groupCache = new Dictionary<string, Group>();
    private Dictionary<string, Space> _spaceCache = new Dictionary<string, Space>();
    private Dictionary<string, bool> _groupResultReceived = new Dictionary<string, bool>();

    public ResultService(IHostInfo hostInfo) // todo: inject caches
    {
        _hostInfo = hostInfo;
    }

    public void DisplayResults()
    {
        foreach (var kvp in _groupCache.ToList())
        {
            string groupId = kvp.Key;
            IEnumerable<Space> spaces = kvp.Value.Spaces;

            Console.WriteLine($"Group ID: {groupId}");


            foreach (var space in spaces)
            {
                Console.WriteLine($"  Space ID: {space.Id}, Width: {space.Width}, Price: {space.Price}");
            }

            Console.WriteLine();
        }
    }

    public void ReceiveResult(string groupId) 
    {
        _groupResultReceived.Add(groupId, true);
    }

    public bool HasFinishedCollectedResults()
    {
        return _groupResultReceived.Values.Count() == _groupCache.Count();
    }

    public bool HasMoreIterations()
    {
        return iteration < _hostInfo.NbOfIteration;
    }

    public void IncrementIteration()
    {
        _groupResultReceived.Clear(); // ish

        Interlocked.Increment(ref iteration);
    }
}
