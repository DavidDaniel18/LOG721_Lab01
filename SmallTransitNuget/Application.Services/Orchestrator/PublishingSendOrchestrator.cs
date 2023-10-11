using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Send;
using Domain.Services.Send.Publishing;
using MessagePack;

namespace Application.Services.Orchestrator;

internal sealed class PublishingSendOrchestrator<TContract> : SendOrchestrator<PublishContext, SerializedPublishMessage>
{
    private protected override PublishContext Context { get; }

    internal PublishingSendOrchestrator(PublishContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PublishWrapper<TContract> publishWrapper) => await Serialize(publishWrapper).BindAsync(Send);

    private static Result<SerializedPublishMessage> Serialize(PublishWrapper<TContract> publishWrapper)
    {
        try
        {
            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var serializedRoutingKey = MessagePackSerializer.Serialize(publishWrapper.RoutingKey);
            var serializedPayloadType = MessagePackSerializer.Serialize(typeof(TContract).Name);
            var serializedPayload = MessagePackSerializer.Serialize(publishWrapper.Payload, options);

            return Result.Success(new SerializedPublishMessage(serializedRoutingKey, serializedPayloadType, serializedPayload));
        }
        catch (Exception e)
        {
            return Result.Failure<SerializedPublishMessage>(e);
        }
    }
}