namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Close : Protocol
{
    private Close() : base(MessageTypesEnum.Close)
    {
    }
}