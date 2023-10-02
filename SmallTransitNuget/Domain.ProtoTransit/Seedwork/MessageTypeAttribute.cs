namespace Domain.ProtoTransit.Seedwork;

internal sealed class MessageTypeAttribute<TMessage> : Attribute where TMessage : ProtoTransit
{
    internal Type MessageType { get; } = typeof(TMessage);
}