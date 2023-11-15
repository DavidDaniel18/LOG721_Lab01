using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Messages.Data;

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