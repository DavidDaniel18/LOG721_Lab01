using Domain.ProtoTransit;

namespace Domain.Services.Sending.SeedWork.Saga;

internal interface ISaga<out TMessage>
{
    TMessage GetOriginalPayload();
    Protocol? GetMessage();
    void Ack();
    void Failure();
    void InternalError();
    void ConnectionClosed();
}