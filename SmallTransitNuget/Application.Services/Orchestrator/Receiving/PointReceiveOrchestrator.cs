using Domain.Common.Monads;
using Domain.ProtoTransit.Exceptions;
using Domain.ProtoTransit;
using Domain.Services.Common;
using Domain.Services.Receiving.States;
using Domain.Services.Receiving;
using Domain.Services.Receiving.ClientReceive;
using Domain.Services.Sending.Subscribing.Dto;
using MessagePack;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Sending.Publishing;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Application.Services.Orchestrator.Receiving;

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
        {
            return await DeserializeAndSendToController(payloadBytes)
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

                    var serializedPayloadType = MessagePackSerializer.Serialize(receiveWrapper.ResultType.Name, options);

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

            var payloadObject = MessagePackSerializer.Deserialize(contract, payload.SerializedPayloadType, options);

            var senderId = MessagePackSerializer.Deserialize<string>(payload.SenderId, options)!;

            return Result.Success(new ReceiveWrapper(senderId, payload, contract, returnType));
        }
        catch (Exception e)
        {
            return Result.Failure<ReceiveWrapper>(e);
        }
    }

    private async Task<Result> AnswerClient(Protocol result) => await result.GetBytes().BindAsync(ComHandler.SendMessage);
}