using Domain.Common;
using Domain.ProtoTransit;

namespace Command.Services
{
    public class Communication
    {
        public Result EstablishConnection()
        {
            var preflight = MessageFactory.Create(MessageTypesEnum.HandShake);


        }

    }
}