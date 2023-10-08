using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Exceptions;
using Domain.Services.Receive.States;
using Domain.Services.Send;
using MessagePack;

namespace Domain.Services.Receive.BrokerReceive;

internal sealed class BrokerReceiveOrchestrator : ReceiveOrchestrator<BrokerReceiveContext, Protocol, BrokerReceiveResult, PublishByteWrapper>
{
    private readonly IReceiveComHandler<BrokerReceiveWrapper> _receiveComHandler;
    private protected override BrokerReceiveContext Context { get; }

    public BrokerReceiveOrchestrator(BrokerReceiveContext context, IComHandler comHandler, IReceiveComHandler<BrokerReceiveWrapper> receiveComHandler) : base(comHandler)
    {
        Context = context;
        _receiveComHandler = receiveComHandler;
    }

    internal async Task<Result<BrokerReceiveResult>> Execute()
    {
        try
        {
            var bytesArray = Array.Empty<byte>();

            await foreach (var bytes in _receiveComHandler.GetAccumulator())
            {
                bytesArray = bytesArray.Concat(bytes).ToArray();

                var parsingResult = Protocol.TryParseMessage(bytesArray);

                if (parsingResult.IsFailure())
                {
                    if (parsingResult.Exception is MessageIncompleteException) continue;

                    return Result.FromFailure<BrokerReceiveResult>(parsingResult);
                }

                bytesArray = parsingResult.Content!.Reminder;

                var brokerReceiveResult = Context.Handle(parsingResult.Content!.Protocol);

                if (brokerReceiveResult.IsFailure()) return Result.FromFailure<BrokerReceiveResult>(brokerReceiveResult);

                var result = brokerReceiveResult.Content!;

                if (result.ShouldReturn) return await AnswerClient(result).BindAsync(() => Result.Success(result));

                var handleMessageResult = await BrokerHandleMessage(result).BindAsync(async () => await AnswerClient(result));

                if (handleMessageResult.IsFailure()) return Result.FromFailure<BrokerReceiveResult>(handleMessageResult);
            }

            return Result.Failure<BrokerReceiveResult>("Client disconnected");
        }
        catch (Exception e)
        {
            return Result.Failure<BrokerReceiveResult>(e);

        }
    }

    private async Task<Result> BrokerHandleMessage(BrokerReceiveResult brokerReceiveResult)
    {
        if (brokerReceiveResult.Result is { } publishByteWrapper)
        {
            return await DeserializeAndSendToController(publishByteWrapper);
        }

        return Result.Success();
    }

    private async Task<Result> DeserializeAndSendToController(PublishByteWrapper publishByteWrapper)
    {
        return await Deserialize(
                publishByteWrapper.SerializedRoutingKey,
                publishByteWrapper.SerializedPayloadType,
                publishByteWrapper.SerializedPayload)
            .BindAsync(async deserialized =>
            {
                try
                {
                    await _receiveComHandler.SendToController(deserialized);
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

    private static Result<BrokerReceiveWrapper> Deserialize(byte[] routingKey, byte[] payloadType, byte[] payload)
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
}