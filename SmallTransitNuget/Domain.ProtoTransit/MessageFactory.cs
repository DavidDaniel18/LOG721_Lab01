namespace Domain.ProtoTransit;

internal static class MessageFactory
{
    internal static Protocol Create(MessageTypesEnum messageType)
    {
        return Protocol.ProtoTransitFactory[messageType].Invoke();
    }
}