using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Commands.Map.Input;
using Domain.Grouping;
using Application.Commands.Result;

namespace Application.Commands.Map.Event;

public sealed class ResultFinishedEventHandler : ICommandHandler<ResultFinishedEvent>
{
    private readonly IHostInfo _hostInfo;
    private Results _results; 

    private readonly IMessagePublisher<InputCommand> _publisher;

    
  

    public ResultFinishedEventHandler(IHostInfo hostInfo, IMessagePublisher<InputCommand> publisher, Results results)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
        _results = results;
    }

    public Task Handle(ResultFinishedEvent command, CancellationToken cancellation)
    {


        bool isLastIteration = !_results.HasMoreIterations();

        if (!isLastIteration)
        {
            var finalResults = _results.GetResults();
            _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName, finalResults), _hostInfo.InputRoutingKey);
        }
        else
        {
            _results.DisplayResults(_results.GetResults());
        }

        return Task.CompletedTask;
    }
}