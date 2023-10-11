using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.StateHolder;

namespace Domain.Services.Send.SeedWork.Saga;

internal sealed class Saga<TContext, TPayload> : ISaga<TPayload> where TContext : SendStateHolder<TContext, TPayload>
{
    private readonly TContext _context;
    private readonly TPayload _originalPayload;
    private readonly SagaItem<TContext, TPayload> _sagaItem;

    internal Saga(TContext context, SagaItem<TContext, TPayload> sagaItem, TPayload originalPayload)
    {
        _context = context;
        _sagaItem = sagaItem;
        _originalPayload = originalPayload;
    }

    public TPayload GetOriginalPayload() => _originalPayload;

    public Protocol GetMessage()
    {
        return _sagaItem.Message;
    }

    public void Success()
    {
        _context.SetState(_sagaItem.OnAck());
    }

    public void Failure()
    {
        _context.SetState(_sagaItem.OnNack());
    }

    public void InternalError()
    {
        _context.SetState(_sagaItem.OnInternalError());
    }

    public void ConnectionClosed()
    {
        _context.SetState(_sagaItem.OnConnectionClosed());
    }
}