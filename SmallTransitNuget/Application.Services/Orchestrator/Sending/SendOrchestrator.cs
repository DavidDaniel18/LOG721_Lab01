using MessagePack;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Sending;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.Send;

namespace SmallTransit.Application.Services.Orchestrator.Sending;

internal sealed class SendOrchestrator<TContract, TResult> : SendingOrchestrator<SendingContext, SerializedSendMessage>
{
    private protected override SendingContext Context { get; }

    internal SendOrchestrator(SendingContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result<TResult>> Execute(SendWrapper<TContract> sendingWrapper) => await Serialize(sendingWrapper).BindAsync(SendAndGetResponse);

    private static Result<SerializedSendMessage> Serialize(SendWrapper<TContract> sendingWrapper)
    {
        try
        {
            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var senderId = MessagePackSerializer.Serialize(sendingWrapper.SenderId);
            var serializedPayloadType = MessagePackSerializer.Serialize(typeof(TContract).GetTypeName());
            var serializedPayload = MessagePackSerializer.Serialize(sendingWrapper.Payload, options);

            return Result.Success(new SerializedSendMessage(senderId, serializedPayloadType, serializedPayload));
        }
        catch (Exception e)
        {
            return Result.Failure<SerializedSendMessage>(e);
        }
    }

    private async Task<Result<TResult>> SendAndGetResponse(SerializedSendMessage stateParameter)
    {
        if (await PrimeConnection(stateParameter) is { } primingResult && primingResult.IsFailure()) return Result.FromFailure<TResult>(primingResult);

        return await Context.BuildSaga(stateParameter).BindAsync<Saga<SendingContext, SerializedSendMessage>, TResult>(WireAndGetResponse);
    }

    private async Task<Result<TResult>> WireAndGetResponse(ISaga<SerializedSendMessage> saga)
    {
        if (saga.GetMessage() is { } message)
        {
            var result = await Serialize(message).BindAsync(ComHandler.SendMessage);

            if (result.IsFailure()) return Result.FromFailure<TResult>(result);

            var resultValue = await WaitForValidResponse(saga);

            if (resultValue.IsFailure()) return Result.FromFailure<TResult>(resultValue);
            
            return resultValue;
        }

        return Result.Failure<TResult>("Saga is not in a valid state");
    }

    private Result<byte[]> Serialize(Protocol message) => message.GetBytes();

    private async Task<Result<TResult>> WaitForValidResponse(ISaga<SerializedSendMessage> saga)
    {
        do
        {
            Protocol? protocolMessage = null;

            var waitForResponseResult = await ComHandler.WaitForResponse(bytes =>
            {
                var messageResult = Protocol.TryParseMessage(bytes);

                if (messageResult.IsSuccess())
                {
                    protocolMessage = messageResult.Content!.Protocol;

                    return Result.Success<byte[]>(messageResult.Content!.Reminder);
                }

                return Result.FromFailure<byte[]>(messageResult);
            });

            if (waitForResponseResult.IsFailure())
            {
                if (waitForResponseResult.Exception is MessageIncompleteException) continue;

                await HandleFailure(saga.InternalError, saga);

                return Result.FromFailure<TResult>(waitForResponseResult);
            }

            return protocolMessage switch
            {
                PayloadResponse payloadResponse => Deserialize(payloadResponse).Bind(response =>
                {
                    saga.PayloadResponse();
                    return response;
                }),
                Ack => await HandleFailure(saga.Ack, saga),
                Nack => await HandleFailure(saga.Failure, saga),
                Close => await HandleFailure(saga.ConnectionClosed, saga),
                _ => await HandleFailure(saga.InternalError, saga)
            };

        } while (true);
    }

    private Result<TResult> Deserialize(PayloadResponse payloadResponse)
    {
        var payloadBytes = payloadResponse.TryGetProperty<Payload>();
        var payloadTypeBytes = payloadResponse.TryGetProperty<PayloadType>();

        if (Result.From(payloadTypeBytes, payloadBytes) is { } combinedResult && combinedResult.IsFailure())
        {
            return Result.FromFailure<TResult>(combinedResult);
        }

        var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

        var typeName = MessagePackSerializer.Deserialize<string>(payloadTypeBytes.Content!.Bytes, options);

        if (typeName != typeof(TResult).GetTypeName()) return Result.Failure<TResult>("Payload type does not match");

        var deserializedPayload = MessagePackSerializer.Deserialize<TResult>(payloadBytes.Content!.Bytes, options);

        return Result.Success(deserializedPayload);
    }

    private new async Task<Result<TResult>> HandleFailure(Action stateCompensatingAction, ISaga<SerializedSendMessage> saga)
    {
        stateCompensatingAction.Invoke();

        return await SendAndGetResponse(saga.GetOriginalPayload());
    }
}