

using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Commands.Map.Input;
using Domain.Grouping;

namespace Application.Commands.Map.Event;

public sealed class ReduceFinishedEventHandler : ICommandHandler<ReduceFinishedEvent>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<InputCommand> _publisher;

    // DI data cache
    // _spaceCache
    private IEnumerable<Group> _groupsCache = new List<Group>(); // todo: link to cache

    private Dictionary<string, bool> _groupFinished = new Dictionary<string, bool>(); 

    public ReduceFinishedEventHandler(IHostInfo hostInfo, IMessagePublisher<InputCommand> publisher)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(ReduceFinishedEvent command, CancellationToken cancellation)
    {
        // todo: do operation

        bool isFinished = false;
        bool isLastIteration = false;

        // ...

        if (isFinished)
        {
            // todo: print data

            if (isLastIteration)
            {
                // todo: print end of map reduce data
            } 
            else
            {
                // send message to input master to trigger another iteration
                _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            }
        }
        
        return Task.CompletedTask;
    }
}