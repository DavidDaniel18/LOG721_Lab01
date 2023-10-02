using Domain.Common;
using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Seedwork;
using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data.Subscribe.Header;

internal sealed partial class SubscribeHeader : ProtoHeader
{
    internal Result<MessagePortion[]> GetSpecificPublishHeaderInfos()
    {
        var routingKeyLength = GetHeaderValue(nameof(RoutingKeyLength));
        var payloadTypeLength = GetHeaderValue(nameof(PayloadTypeLength));
        var queueNameLength = GetHeaderValue(nameof(QueueNameLength));

        if (Result.IsFailure(routingKeyLength, payloadTypeLength, queueNameLength))
            return Result.Failure<MessagePortion[]>(new Exception("Could not get specific header infos."));

        var totalHeaderLength = GetTotalHeaderLength();

        var routingKeyInfo = new MessagePortion(totalHeaderLength, routingKeyLength.Content);
        var payloadTypeInfo = new MessagePortion(totalHeaderLength + routingKeyLength.Content, payloadTypeLength.Content);
        var payloadSizeInfo = new MessagePortion(totalHeaderLength + routingKeyLength.Content + payloadTypeLength.Content, queueNameLength.Content);

        return Result.Success(new[] { routingKeyInfo, payloadTypeInfo, payloadSizeInfo });
    }
}