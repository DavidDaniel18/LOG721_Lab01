using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class MessageService : IMessageService
    {
        IChannelService _channelService;

        public MessageService(IChannelService channelService)
        {
            _channelService = channelService;
        }

        public void AddMessageToQueue(string queueName, IPublication publication)
        {
            _channelService.AddMessage(queueName, publication);
        }
    }
}
