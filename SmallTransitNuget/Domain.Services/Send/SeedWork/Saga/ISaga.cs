using Domain.ProtoTransit;

namespace Domain.Services.Send.SeedWork.Saga;

internal interface ISaga<out TMessage>
{
    TMessage GetOriginalPayload();
    Protocol? GetMessage();
    void Success();
    void Failure();
    void InternalError();
    void ConnectionClosed();
}