using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Exceptions;
using Domain.Services.Common;
using Domain.Services.Common.Exceptions;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.StateHolder;

namespace Domain.Services.Send;

internal abstract class SendOrchestrator<TContext, TStateParameter> where TContext : SendStateHolder<TContext, TStateParameter>
{
    private readonly IComHandler _comHandler;

    private protected abstract TContext Context { get; }

    protected SendOrchestrator(IComHandler comHandler)
    {
        _comHandler = comHandler;
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
        while (saga.GetMessage() is { } message)
        {
            var result = await Serialize(message).BindAsync(_comHandler.SendMessage).BindAsync(() => WaitForValidResponse(saga));

            if (result.IsFailure()) return result;

            saga.Success();
        }

        return Result.Success();
    }

    private Result<byte[]> Serialize(Protocol message) => message.GetBytes();

    private async Task<Result> WaitForValidResponse(ISaga<TStateParameter> saga)
    {
        do
        {
            Protocol? protocolMessage = null;

            var waitForResponseResult = await _comHandler.WaitForResponse(bytes =>
            {
                var messageResult = Protocol.TryParseMessage(bytes);

                if (messageResult.IsSuccess())
                {
                    protocolMessage = messageResult.Content!.Protocol;

                    return Result.Success(messageResult.Content!.Reminder);
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

    private async Task<Result> HandleFailure(Action stateCompensatingAction, ISaga<TStateParameter> saga)
    {
        stateCompensatingAction.Invoke();

        return await Send(saga.GetOriginalPayload());
    }
}