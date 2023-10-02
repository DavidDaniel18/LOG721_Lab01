using Domain.Common;
using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Seedwork;

namespace Domain.ProtoTransit;

public abstract partial class ProtoTransit
{
    public static Result<ProtoTransit> TryParseMessage(byte[] message)
    {
        var messageTypeResult = ProtoHeader.ParseMessageTypeFromBytes(message);

        if (messageTypeResult.IsFailure()) return Result.FromFailure<ProtoTransit>(messageTypeResult);

        var protoTransitMessage = ProtoTransitFactory[messageTypeResult.Content].Invoke();

        var headerInitializationResult = protoTransitMessage.Header.InitializeFromBytes(message);

        if (headerInitializationResult.IsFailure()) return Result.FromFailure<ProtoTransit>(headerInitializationResult);

        return protoTransitMessage.InitializeFrom(message);
    }

    private Result<ProtoTransit> InitializeFrom(byte[] message)
    {
        var headerInfos = Header.GetPropertyHeaderInfos(_protoProperties.Values.ToArray());

        if (headerInfos.IsFailure()) return Result.FromFailure<ProtoTransit>(headerInfos);

        for (var index = 0; index < headerInfos.Content.Count; index++)
        {
            var headerInfo = headerInfos.Content[index];
            var propertyResult = TryGetProperty(headerInfo.PropertyType);

            if (propertyResult.IsFailure()) return Result.FromFailure<ProtoTransit>(propertyResult);

            propertyResult.Content!.Bytes = Parse(headerInfos.Content![index]);
        }

        return Result.Success(this);

        byte[] Parse(MessagePortion messagePortion) => message.Skip(messagePortion.BeginAtIndex).Take(messagePortion.Length).ToArray();
    }
}