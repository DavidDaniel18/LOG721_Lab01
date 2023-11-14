using Domain.Publicity;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class MapFinishedEventController : IConsumer<Space>
{
    public Task Consume(Space contract)
    {
        throw new NotImplementedException();
    }
}
