using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Exceptions;
using Domain.Services.Common;
using Domain.Services.Receive;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Receive.States;
using MessagePack;

namespace Application.Services.Orchestrator;

internal sealed class ClientReceiveOrchestrator<TContract> : ReceiveOrchestrator<ClientReceiveContext, Protocol, ClientReceiveResult, byte[]>
{
    private readonly IControllerDelegate<TContract> _controllerDelegate;
    private protected override ClientReceiveContext Context { get; }

    internal ClientReceiveOrchestrator(ClientReceiveContext context, IComHandler comHandler, IControllerDelegate<TContract> controllerDelegate) : base(comHandler)
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

    private async Task<Result> BrokerHandleMessage(ClientReceiveResult clientReceiveResult)
    {
        if (clientReceiveResult.Result is { } payloadBytes)
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

    private async Task<Result> AnswerClient(ClientReceiveResult result) => await result.Response.GetBytes().BindAsync(ComHandler.SendMessage);
}