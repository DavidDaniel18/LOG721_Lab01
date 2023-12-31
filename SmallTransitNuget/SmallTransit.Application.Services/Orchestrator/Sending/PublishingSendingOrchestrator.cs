﻿using MessagePack;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Sending;
using SmallTransit.Domain.Services.Sending.Publishing;

namespace SmallTransit.Application.Services.Orchestrator.Sending;

internal sealed class PublishingSendingOrchestrator<TContract> : SendingOrchestrator<PublishContext, SerializedPublishMessage>
{
    private protected override PublishContext Context { get; }

    internal PublishingSendingOrchestrator(PublishContext context, IComHandler comHandler) : base(comHandler)
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