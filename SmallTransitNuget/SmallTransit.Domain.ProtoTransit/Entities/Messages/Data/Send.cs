using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Send : Protocol
{
    public Send() : base(MessageTypesEnum.Send)
    {
        AddProperty<SenderId>();
        AddProperty<PayloadType>();
        AddProperty<Payload>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}