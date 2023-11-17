using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;

namespace SmallTransit.Domain.Services.Sending.SeedWork.Saga;

internal sealed class Saga<TContext, TPayload> : ISaga<TPayload> where TContext : SendingStateHolder<TContext, TPayload>
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

    public void Ack()
    {
        _context.SetState(_sagaItem.OnAck());
    }

    public void PayloadResponse()
    {
        _context.SetState(_sagaItem.OnPayloadResponse());
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