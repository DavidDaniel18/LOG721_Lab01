using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Common.Exceptions;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;

namespace SmallTransit.Domain.Services.Sending;

internal abstract class SendingOrchestrator<TContext, TStateParameter> where TContext : SendingStateHolder<TContext, TStateParameter>
{
    private protected readonly IComHandler ComHandler;

    private protected abstract TContext Context { get; }

    protected SendingOrchestrator(IComHandler comHandler)
    {
        ComHandler = comHandler;
    }

    private protected async Task<Result> Send(TStateParameter stateParameter)
    {
        if (await PrimeConnection(stateParameter) is { } primingResult && primingResult.IsFailure()) return Result.FromFailure(primingResult);

        return await Context.BuildSaga(stateParameter).BindAsync(Wire);
    }

    private protected async Task<Result> PrimeConnection(TStateParameter serializedPayload)
    {
        while (Context.GetConnectionReady() is false)
        {
            var result = await Context.BuildSaga(serializedPayload).BindAsync(Wire);

            if (result.Exception is ConnectionClosedException) return result;
        }

        return Result.Success();
    }

    private protected async Task<Result> Wire(ISaga<TStateParameter> saga)
    {
        if (saga.GetMessage() is { } message)
        {
            var result = await Serialize(message).BindAsync(ComHandler.SendMessage).BindAsync(() => WaitForValidResponse(saga));

            if (result.IsFailure()) return result;

            saga.Ack();
        }

        return Result.Success();
    }

    private Result<byte[]> Serialize(Protocol message) => message.GetBytes();

    private async Task<Result> WaitForValidResponse(ISaga<TStateParameter> saga)
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

                return waitForResponseResult;
            }

            return protocolMessage switch
            {
                Ack => Result.Success(),
                Nack => await HandleFailure(saga.Failure, saga),
                Close => await HandleFailure(saga.ConnectionClosed, saga),
                _ => await HandleFailure(saga.InternalError, saga)
            };

        } while (true);
    }

    private protected async Task<Result> HandleFailure(Action stateCompensatingAction, ISaga<TStateParameter> saga)
    {
        stateCompensatingAction.Invoke();

        return await Send(saga.GetOriginalPayload());
    }
}