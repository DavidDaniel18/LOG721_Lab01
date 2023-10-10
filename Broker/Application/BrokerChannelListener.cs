using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
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
        private readonly IEndpoint _endPoint;

        public BrokerChannelListener(Channel<IPublication> channel, IEndpoint endPoint) 
        { 
            _channel = channel;
            _endPoint = endPoint;
        }

        public async void Listen()
        {
            while (await _channel.Reader.WaitToReadAsync())
            {
                while (_channel.Reader.TryRead(out var publication))
                {
                    _endPoint.Publish(publication);
                }
            }
        }

        // todo: validate error handling.
    }
}
