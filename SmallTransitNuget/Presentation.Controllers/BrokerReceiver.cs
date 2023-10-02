using System.Threading.Channels;

namespace Presentation.Controllers;

public sealed class BrokerReceiver
{
    public Result<ChannelReader<Payload<TContract>>> Receive<TContract>(ConnectionContext payload) where TContract : class
    {
        var input = payload.Transport.Input.AsStream();
        var output = payload.Transport.Input.AsStream();
    }
}