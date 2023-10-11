using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Messages.Data;

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