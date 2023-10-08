﻿using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Subscribe : Protocol
{
    private Subscribe() : base(MessageTypesEnum.Subscribe)
    {
        AddProperty<RoutingKey>();
        AddProperty<PayloadType>();
        AddProperty<QueueName>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}