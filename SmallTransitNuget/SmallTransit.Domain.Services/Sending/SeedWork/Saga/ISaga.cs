using SmallTransit.Domain.ProtoTransit;

namespace SmallTransit.Domain.Services.Sending.SeedWork.Saga;

internal interface ISaga<out TMessage>
{
    TMessage GetOriginalPayload();
    Protocol? GetMessage();
    void Ack();
    void PayloadResponse();
    void Failure();
    void InternalError();
    void ConnectionClosed();
}