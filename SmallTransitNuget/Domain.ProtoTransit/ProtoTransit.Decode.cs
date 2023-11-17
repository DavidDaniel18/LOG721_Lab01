using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit.Entities.Header;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.ProtoTransit.Seedwork;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit;

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
                ? Result.FromFailure<(Protocol Protocol, int Length)>(headerInitializationResult)
                : protoTransitMessage.InitializeFrom(message);
        })
            .Bind(tuple => Result.Success((tuple.Protocol, message[(tuple.Length.Equals(0) ? messageSize : tuple.Length)..])));
    }

    private Result<(Protocol Protocol, int Length)> InitializeFrom(byte[] message)
    {
        var result = Header.GetPropertyHeaderInfos(_protoProperties.Values.ToArray());

        if (result.IsFailure()) return Result.FromFailure<(Protocol Protocol, int Length)>(result);

        for (var index = 0; index < result.Content!.Count; index++)
        {
            var messagePortion = result.Content![index];

            var propertyResult = TryGetProperty(messagePortion.PropertyType);

            if (propertyResult.IsFailure()) return Result.FromFailure<(Protocol Protocol, int Length)>(propertyResult);

            propertyResult.Content!.Bytes = Parse(messagePortion);

            if(index.Equals(result.Content!.Count-1)) return Result.Success((this, messagePortion.BeginAtIndex + messagePortion.Length));
        }

        return Result.Success((this, 0));

        byte[] Parse(MessagePortion messagePortion)
        {
            if (messagePortion.Length.Equals(0))
                throw new Exception("Message portion length is 0");

            if (messagePortion.BeginAtIndex + messagePortion.Length > message.Length)
                throw new Exception("Message portion length is greater than message length");

            return message[messagePortion.BeginAtIndex..(messagePortion.Length + messagePortion.BeginAtIndex)];
        }
    }
}