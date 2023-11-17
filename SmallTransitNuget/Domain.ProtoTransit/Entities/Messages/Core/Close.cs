namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Close : Protocol
{
    public Close() : base(MessageTypesEnum.Close)
    {
    }
}