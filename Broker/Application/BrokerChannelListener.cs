using Domain.Common;
using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using MessagePack;
using SmallTransit.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

        public BrokerChannelListener(Channel<IPublication> channel, IBrokerPushEndpoint endPoint) 
        { 
            _channel = channel;
            _endPoint = endPoint;
        }

        public async Task<Result> Listen()
        {
            while (await _channel.Reader.WaitToReadAsync())
            {
                while (_channel.Reader.TryRead(out var publication))
                {
                    Result result = await _endPoint.Push(publication.Message);

                    if (result.IsFailure())
                    {
                        return result;
                    }
                }
            }
            return Result.Failure("Stopped waiting for message within the broker");
        }
    }
}
