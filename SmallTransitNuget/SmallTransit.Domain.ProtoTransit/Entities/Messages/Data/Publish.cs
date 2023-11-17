using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Publish : Protocol
{
    public Publish() : base(MessageTypesEnum.Publish)
    {
        AddProperty<RoutingKey>();
        AddProperty<PayloadType>();
        AddProperty<Payload>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}