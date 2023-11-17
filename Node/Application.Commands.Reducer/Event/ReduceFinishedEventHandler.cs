

using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Commands.Map.Input;
using Application.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Map.Event;

public sealed class ReduceFinishedEventHandler : ICommandHandler<ReduceFinishedEvent>
{
    private readonly ILogger<ReduceFinishedEventHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<InputCommand> _publisher;

    private readonly IResultService _resultService;

    public ReduceFinishedEventHandler(ILogger<ReduceFinishedEventHandler> logger, IHostInfo hostInfo, IMessagePublisher<InputCommand> publisher, IResultService resultService)
    {
        _logger = logger;
        _hostInfo = hostInfo;
        _publisher = publisher;
        _resultService = resultService;
    }

    public Task Handle(ReduceFinishedEvent command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");

        _resultService.ReceiveResult(command.group.Id, command.delta);

        if (_resultService.HasFinishedCollectedResults())
        {
            _logger.LogInformation($"Finished iteration");

            _resultService.IncrementIteration();
            _resultService.DisplayResults();

            if (_resultService.HasMoreIterations())
            {
                _logger.LogInformation($"Start next iteration");
                // send message to input master to trigger another iteration
                _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            } 
            else
            {
                Console.WriteLine("Last iteration finished...\nMap Reduce terminated");
                _resultService.DisplayResults();
            }
        }
        else
        {
            _logger.LogInformation("Not finished yet... still waiting for other results...");
        }
        
        return Task.CompletedTask;
    }
}