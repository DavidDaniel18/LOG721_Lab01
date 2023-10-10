using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Exceptions;
using Domain.Services.Common;
using Domain.Services.Receive;
using Domain.Services.Receive.BrokerReceive;
using Domain.Services.Receive.States;
using Domain.Services.Send.Subscribing.Dto;
using MessagePack;

namespace Application.Services.Orchestrator;

internal sealed class BrokerReceiveOrchestrator : ReceiveOrchestrator<BrokerReceiveContext, Protocol, BrokerReceiveResult, ReceivePublishByteWrapper>
{
    private readonly IControllerDelegate<BrokerReceiveWrapper> _controllerDelegate;
    private protected override BrokerReceiveContext Context { get; }

    public BrokerReceiveOrchestrator(BrokerReceiveContext context, IComHandler comHandler, IControllerDelegate<BrokerReceiveWrapper> controllerDelegate) : base(comHandler)
    {
        Context = context;
        _controllerDelegate = controllerDelegate;
    }

    internal async Task<Result<SubscriptionWrapper>> Execute()
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

                    return Result.FromFailure<SubscriptionWrapper>(parsingResult);
                }

                bytesArray = parsingResult.Content!.Reminder;

                var brokerReceiveResult = Context.Handle(parsingResult.Content!.Protocol);

                if (brokerReceiveResult.IsFailure()) return Result.FromFailure<SubscriptionWrapper>(brokerReceiveResult);

                var result = brokerReceiveResult.Content!;

                if (result.ShouldReturn) 
                    return await AnswerClient(result)
                        .BindAsync(() => DeserializeSubscriptionWrapper(
                            result.SubscriptionDto!.RoutingKey,
                            result.SubscriptionDto!.PayloadType, 
                            result.SubscriptionDto!.QueueName));

                var handleMessageResult = await BrokerHandleMessage(result).BindAsync(async () => await AnswerClient(result));

                if (handleMessageResult.IsFailure()) return Result.FromFailure<SubscriptionWrapper>(handleMessageResult);
            }

            return Result.Failure<SubscriptionWrapper>("Client disconnected");
        }
        catch (Exception e)
        {
            return Result.Failure<SubscriptionWrapper>(e);

        }
    }

    private async Task<Result> BrokerHandleMessage(BrokerReceiveResult brokerReceiveResult)
    {
        if (brokerReceiveResult.Result is { } publishByteWrapper)
        {
            return await DeserializeAndSendToController(publishByteWrapper);
        }

        return Result.Failure("The broker received a null result");
    }

    private async Task<Result> DeserializeAndSendToController(ReceivePublishByteWrapper receivePublishByteWrapper)
    {
        return await DeserializeBrokerReceiveWrapper(
                receivePublishByteWrapper.SerializedRoutingKey,
                receivePublishByteWrapper.SerializedPayloadType,
                receivePublishByteWrapper.SerializedPayload)
            .BindAsync(async deserialized =>
            {
                try
                {
                    await _controllerDelegate.SendToController(deserialized);

                    return Result.Success();
                }
                catch (Exception e)
                {
                    return Result.Failure(e);
                }
            });
    }

    private async Task<Result> AnswerClient(BrokerReceiveResult result)
        => await result.Response.GetBytes().BindAsync(ComHandler.SendMessage);

    private static Result<BrokerReceiveWrapper> DeserializeBrokerReceiveWrapper(byte[] routingKey, byte[] payloadType, byte[] payload)
    {
        try
        {
            return Result.Success(
                new BrokerReceiveWrapper(
                    MessagePackSerializer.Deserialize<string>(routingKey),
                    MessagePackSerializer.Deserialize<string>(payloadType),
                    payload));
        }
        catch (Exception e)
        {
            return Result.Failure<BrokerReceiveWrapper>(e);
        }
    }

    private static Result<SubscriptionWrapper> DeserializeSubscriptionWrapper(byte[] routingKey, byte[] payloadType, byte[] queueName)
    {
        try
        {
            return Result.Success(
                new SubscriptionWrapper(
                    MessagePackSerializer.Deserialize<string>(routingKey),
                    MessagePackSerializer.Deserialize<string>(payloadType),
                    MessagePackSerializer.Deserialize<string>(queueName)));
        }
        catch (Exception e)
        {
            return Result.Failure<SubscriptionWrapper>(e);
        }
    }
}