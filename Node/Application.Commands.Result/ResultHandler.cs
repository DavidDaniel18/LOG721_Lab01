using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Result
{
    internal class ResultHandler : ICommandHandler<ResultsCommand>
    {
        private readonly IHostInfo _hostInfo;
        private readonly IMessagePublisher<ResultFinishedEvent> _messagePublisher;
        
        public ResultHandler(IHostInfo hostInfo, IMessagePublisher<ResultFinishedEvent> messagePublisher)
        {
            _hostInfo = hostInfo;
            _messagePublisher = messagePublisher;
        }

        public Task Handle(ResultsCommand command, CancellationToken cancel) {
            _messagePublisher.PublishAsync(new ResultFinishedEvent(command.group), _hostInfo.MapFinishedEventRoutingKey);

            return Task.CompletedTask;
        }
    }
}
