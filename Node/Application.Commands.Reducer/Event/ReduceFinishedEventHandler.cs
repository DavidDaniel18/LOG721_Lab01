using Application.Commands.Interfaces;
using Application.Commands.Map.Input;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Reducer.Event;

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

    public async Task Handle(ReduceFinishedEvent command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");

        await _resultService.ReceiveResult(command.group.Id, command.delta);

        if (await _resultService.HasFinishedCollectedResults())
        {
            _logger.LogInformation($"Finished iteration");

            await _resultService.IncrementIteration();
            
            await _resultService.DisplayResults();

            if (await _resultService.HasMoreIterations())
            {
                _logger.LogInformation($"Start next iteration");
                // send message to input master to trigger another iteration
                await _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            } 
            else
            {
                Console.WriteLine("Last iteration finished...\nMap Reduce terminated");
                await _resultService.DisplayResults();
            }
        }
        else
        {
            _logger.LogInformation("Not finished yet... still waiting for other results...");
        }
    }
}