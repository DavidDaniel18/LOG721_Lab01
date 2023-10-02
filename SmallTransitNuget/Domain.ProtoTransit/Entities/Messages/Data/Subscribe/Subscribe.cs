using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Entities.Messages.Data.Subscribe.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data.Subscribe;

public sealed partial class Subscribe : ProtoTransit
{
    private Subscribe() : base(new SubscribeHeader()) { }
}