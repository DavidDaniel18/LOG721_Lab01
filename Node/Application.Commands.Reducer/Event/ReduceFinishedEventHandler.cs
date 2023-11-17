

using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Commands.Map.Input;
using Application.Commands.Interfaces;
using Domain.Common.Monads;

namespace Application.Commands.Map.Event;

public sealed class ReduceFinishedEventHandler : ICommandHandler<ReduceFinishedEvent>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<InputCommand> _publisher;

    private readonly IResultService _resultService;

    public ReduceFinishedEventHandler(IHostInfo hostInfo, IMessagePublisher<InputCommand> publisher, IResultService resultService)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
        _resultService = resultService;
    }

    public Task Handle(ReduceFinishedEvent command, CancellationToken cancellation)
    {
        _resultService.ReceiveResult(command.group.Id);

        if (_resultService.HasFinishedCollectedResults())
        {
            bool isLastIteration = !_resultService.HasMoreIterations();

            _resultService.IncrementIteration();

            Task.Run(() => _resultService.DisplayResults());

            if (!isLastIteration)
            {
                // send message to input master to trigger another iteration
                _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            } 
            else
            {
                Console.WriteLine("Last iteration finished...\nMap Reduce terminated");
            }
        }
        
        return Task.CompletedTask;
    }
}