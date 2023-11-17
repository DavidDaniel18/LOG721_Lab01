

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
        _logger.LogInformation($"Reduce Finished Event: {command.group.Id}");
        _resultService.ReceiveResult(command.group.Id);

        if (_resultService.HasFinishedCollectedResults())
        {
            _logger.LogInformation($"Has collected all the result");
            bool isLastIteration = !_resultService.HasMoreIterations();

            _logger.LogInformation($"Is the last iteration: {isLastIteration}");
            _resultService.IncrementIteration();

            _logger.LogInformation($"Display result");
            Task.Run(() => _resultService.DisplayResults());

            if (!isLastIteration)
            {
                _logger.LogInformation($"Not the last iteration, send Input event...");
                // send message to input master to trigger another iteration
                Task.WaitAll(_publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey));
            } 
            else
            {
                Console.WriteLine("Last iteration finished...\nMap Reduce terminated");
            }
        }
        
        return Task.CompletedTask;
    }
}