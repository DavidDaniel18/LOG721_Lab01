using MessagePack;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Receiving.States;
using SmallTransit.Domain.Services.Receiving.SubscriberReceive;

namespace SmallTransit.Application.Services.Orchestrator.Receiving;

internal sealed class SubscriberReceiveOrchestrator<TContract> : ReceiveOrchestrator<SubscriberReceiveContext, Protocol, SubscriberReceiveResult, byte[]>
{
    private readonly IControllerDelegate<TContract> _controllerDelegate;

    private protected override SubscriberReceiveContext Context { get; }

    internal SubscriberReceiveOrchestrator(SubscriberReceiveContext context, IComHandler comHandler, IControllerDelegate<TContract> controllerDelegate) : base(comHandler)
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

                var result = await clientReceiveResult.BindAsync(BrokerHandleMessage).BindAsync(async () => await AnswerClient(clientReceiveResult.Content!));

                if (result.IsFailure()) return Result.FromFailure(result);
            }

            return Result.Failure<BrokerReceiveResult>("Client disconnected");
        }
        catch (Exception e)
        {
            return Result.Failure<BrokerReceiveResult>(e);

        }
    }

    private async Task<Result> BrokerHandleMessage(SubscriberReceiveResult subscriberReceiveResult)
    {
        if (subscriberReceiveResult.Result is { } payloadBytes)
        {
            return await DeserializeAndSendToController(payloadBytes);
        }

        return Result.Success();
    }

    private async Task<Result> DeserializeAndSendToController(byte[] receivePushByteWrapper)
    {
        return await Deserialize(receivePushByteWrapper)
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

    private static Result<TContract> Deserialize(byte[] payload)
    {
        try
        {
            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            return Result.Success(MessagePackSerializer.Deserialize<TContract>(payload, options));
        }
        catch (Exception e)
        {
            return Result.Failure<TContract>(e);
        }
    }

    private async Task<Result> AnswerClient(SubscriberReceiveResult result) => await result.Response.GetBytes().BindAsync(ComHandler.SendMessage);
}