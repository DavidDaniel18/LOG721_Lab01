using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class PayloadResponse : Protocol
{
    public PayloadResponse() : base(MessageTypesEnum.PayloadResponse)
    {
        AddProperty<Payload>();
        AddProperty<PayloadType>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}