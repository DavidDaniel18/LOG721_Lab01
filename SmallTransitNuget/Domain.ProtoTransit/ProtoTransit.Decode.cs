using Domain.Common;
using Domain.ProtoTransit.Entities.Header;

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

    private protected virtual Result<ProtoTransit> InitializeFrom(byte[] message) => Result.Success(this);
}