namespace SmallTransit.Domain.ProtoTransit.Seedwork;

internal sealed class MessageTypeAttribute<TMessage> : Attribute where TMessage : Protocol
{
    internal Type MessageType { get; } = typeof(TMessage);
}