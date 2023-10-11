using Domain.Common;
using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using MessagePack;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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

                if (result.IsFailure())
                {
                    return result;
                }
            }

            return Result.Failure("Stopped waiting for message within the broker");
        }
    }
}
