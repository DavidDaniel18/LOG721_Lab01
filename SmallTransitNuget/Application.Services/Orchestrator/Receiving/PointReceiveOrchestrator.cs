using MessagePack;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Receiving.ClientReceive;
using SmallTransit.Domain.Services.Receiving.States;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Application.Services.Orchestrator.Receiving;

internal sealed class PointReceiveOrchestrator : ReceiveOrchestrator<ClientReceiveContext, Protocol, ClientReceiveResult, ReceiveSendByteWrapper>
{
    private readonly IReceiveControllerDelegate _controllerDelegate;

    private protected override ClientReceiveContext Context { get; }

    internal PointReceiveOrchestrator(ClientReceiveContext context, IComHandler comHandler, IReceiveControllerDelegate controllerDelegate) : base(comHandler)
    {
        _controllerDelegate = controllerDelegate;
        Context = context;
    }

    internal async Task<Result> Execute()
    {
        try
        {
            var bytesArray = Array.Empty<byte>();

            await foreach (var bytes in ComHandler.GetAccumulator())
            {
                bytesArray = bytesArray.Concat(bytes).ToArray();

                var parsingResult = Protocol.TryParseMessage(bytesArray);

                if (parsingResult.IsFailure())
                {
                    if (parsingResult.Exception is MessageIncompleteException) continue;

                    return Result.FromFailure(parsingResult);
                }

                bytesArray = parsingResult.Content!.Reminder;

                var clientReceiveResult = Context.Handle(parsingResult.Content!.Protocol);

                if (clientReceiveResult.IsFailure()) return Result.FromFailure(clientReceiveResult);

                var clientReceive = clientReceiveResult.Content!;

                if(clientReceive.ShouldReturn) 
                    return Result.Success();

                var result = await clientReceiveResult.BindAsync(ReceiverHandleMessage).BindAsync(AnswerClient);

                if (result.IsFailure()) return Result.FromFailure(result);
            }

            return Result.Failure<BrokerReceiveResult>("Client disconnected");
        }
        catch (Exception e)
        {
            return Result.Failure<BrokerReceiveResult>(e);

        }
    }

    private async Task<Result<Protocol>> ReceiverHandleMessage(ClientReceiveResult clientReceiveResult)
    {
        if (clientReceiveResult.Result is { } payloadBytes)
        { return await DeserializeAndSendToController(payloadBytes)
            .BindAsync(tuple => Context.GetPayloadResponse(tuple.payload, tuple.payloadType));
        }

        return Result.Failure<Protocol>("No payload received");
    }

    private async Task<Result<(byte[] payload, byte[] payloadType)>> DeserializeAndSendToController(ReceiveSendByteWrapper receiveSendByteWrapper)
    {
        return await Deserialize(receiveSendByteWrapper)
            .BindAsync(async receiveWrapper =>
            {
                try
                {
                    var result = await _controllerDelegate.SendToController(receiveWrapper);

                    var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

                    var serializedPayload = MessagePackSerializer.Serialize(receiveWrapper.ResultType, result, options);

                    var serializedPayloadType = MessagePackSerializer.Serialize(receiveWrapper.ResultType.GetTypeName(), options);

                    return Result.Success((serializedPayload, serializedPayloadType));
                }
                catch (Exception e)
                {
                    return Result.Failure<(byte[] payload, byte[] payloadType)>(e);
                }
            });
    }

    private Result<ReceiveWrapper> Deserialize(ReceiveSendByteWrapper payload)
    {
        try
        {
            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var typeName = MessagePackSerializer.Deserialize<string>(payload.SerializedPayloadType, options);

            var (contract, returnType) = _controllerDelegate.GetContractType(typeName);

            var payloadObject = MessagePackSerializer.Deserialize(contract, payload.SerializedPayload, options);

            var senderId = MessagePackSerializer.Deserialize<string>(payload.SenderId, options)!;

            return Result.Success(new ReceiveWrapper(senderId, payloadObject, contract, returnType));
        }
        catch (Exception e)
        {
            return Result.Failure<ReceiveWrapper>(e);
        }
    }

    private async Task<Result> AnswerClient(Protocol result) => await result.GetBytes().BindAsync(ComHandler.SendMessage);
}