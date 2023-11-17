using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Subscribe : Protocol
{
    public Subscribe() : base(MessageTypesEnum.Subscribe)
    {
        AddProperty<RoutingKey>();
        AddProperty<PayloadType>();
        AddProperty<QueueName>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}