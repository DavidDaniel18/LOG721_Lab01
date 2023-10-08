using Domain.Common;
using MessagePack;

namespace Domain.Services.Send.Publishing;

internal sealed class PublishingSendOrchestrator : SendOrchestrator<PublishContext, SerializedPublishMessage>
{
    private protected override PublishContext Context { get; }

    internal PublishingSendOrchestrator(PublishContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PublishWrapper publishWrapper) => await Serialize(publishWrapper).BindAsync(Send);

    private static Result<SerializedPublishMessage> Serialize(PublishWrapper publishWrapper)
    {
        try
        {
            var serializedRoutingKey = MessagePackSerializer.Serialize(publishWrapper.routingKey);
            var serializedPayloadType = MessagePackSerializer.Serialize(publishWrapper.payload.GetType());
            var serializedPayload = MessagePackSerializer.Serialize(publishWrapper.payload);

            return Result.Success(new SerializedPublishMessage(serializedRoutingKey, serializedPayloadType, serializedPayload));
        }
        catch (Exception e)
        {
            return Result.Failure<SerializedPublishMessage>(e);
        }
    }
}