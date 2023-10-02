using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Messages.Data;

public sealed class Publish : ProtoTransit
{
    private Publish() : base(MessageTypesEnum.Publish)
    {
        AddProperty<RoutingKey>();
        AddProperty<PayloadType>();
        AddProperty<Payload>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}