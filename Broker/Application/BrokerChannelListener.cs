using Interfaces;
using Interfaces.Domain;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Interfaces;
using System.Threading.Channels;
using SmallTransit.Abstractions.Monads;

namespace Application
{
    public class BrokerChannelListener : IBrokerChannelListener
    {
        private readonly Channel<IPublication> _channel;
        private readonly IBrokerPushEndpoint _endPoint;
        private readonly ILogger _logger;

        public BrokerChannelListener(ILogger logger, Channel<IPublication> channel, IBrokerPushEndpoint endPoint) 
        { 
            _logger = logger;
            _channel = channel;
            _endPoint = endPoint;
        }

        public async Task<Result> Listen()
        {
            await foreach (var publication in _channel.Reader.ReadAllAsync())
            {
                _logger.LogInformation("Listened {0}: {1}, now pushing", publication.Contract, publication.Message);

                Result result = await _endPoint.Push(publication.Message);

                if (result.IsFailure()) return result;
            }

            return Result.Failure("Stopped waiting for message within the broker");
        }
    }
}
