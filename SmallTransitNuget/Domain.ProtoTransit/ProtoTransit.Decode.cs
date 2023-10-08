using Domain.Common;
using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Exceptions;
using Domain.ProtoTransit.Extensions;
using Domain.ProtoTransit.Seedwork;
using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit;

internal abstract partial class Protocol
{
    internal static Result<(Protocol Protocol, byte[] Reminder)> TryParseMessage(byte[] message)
    {
        if (message.Length < (HeaderLengthItem.StorageSizeInBytes + MessageTypeItem.StorageSizeInBytes)) return Result.Failure<(Protocol Protocol, byte[] Reminder)>(new MessageIncompleteException());

        var messageSize = message[..HeaderLengthItem.StorageSizeInBytes].FromBytesToInt();

        if (message.Length < messageSize) return Result.Failure<(Protocol Protocol, byte[] Reminder)>(new MessageIncompleteException());

        return ProtoHeader.ParseMessageTypeFromBytes(message).Bind(messageType =>
        {
            var protoTransitMessage = ProtoTransitFactory[messageType].Invoke();

            var headerInitializationResult = protoTransitMessage.Header.InitializeFromBytes(message);

            return headerInitializationResult.IsFailure()
                ? Result.FromFailure<Protocol>(headerInitializationResult)
                : protoTransitMessage.InitializeFrom(message);
        })
            .Bind(protocol => Result.Success((protocol, message[messageSize..])));
    }

    private Result<Protocol> InitializeFrom(byte[] message)
    {
        return Header.GetPropertyHeaderInfos(_protoProperties.Values.ToArray()).Bind(messagePortions =>
        {
            foreach (var messagePortion in messagePortions)
            {
                var propertyResult = TryGetProperty(messagePortion.PropertyType);

                if (propertyResult.IsFailure()) return Result.FromFailure<Protocol>(propertyResult);

                propertyResult.Content!.Bytes = Parse(messagePortion);
            }

            return Result.Success(this);
        });

        byte[] Parse(MessagePortion messagePortion) => message[messagePortion.BeginAtIndex..(messagePortion.Length + messagePortion.BeginAtIndex)];
    }
}