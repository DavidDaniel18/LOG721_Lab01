﻿using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Push : Protocol
{
    public Push() : base(MessageTypesEnum.Push)
    {
        AddProperty<Payload>();

        var result = Header.SetHeaders(GetProperties());

        result.ThrowIfException();
    }
}