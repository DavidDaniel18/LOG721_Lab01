using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.Services.Common;
using Domain.Services.Receive.States;
using Domain.Services.Send.Push;

namespace Domain.Services.Receive.BrokerReceive.States;

internal sealed class FreeBrokerState : BrokerReceiveState
{
    public FreeBrokerState(BrokerReceiveContext context) : base(context) { }

    internal override Result<BrokerReceiveResult> Handle(Protocol payload)
    {
        switch (payload)
        {
            case HandShake:
                {
                    var returnProtocol = MessageFactory.Create(MessageTypesEnum.Ack);

                    Context.SetState(new BrokerPrimedState(Context));

                    return Result.Success(new BrokerReceiveResult(returnProtocol));
                }
            case Subscribe:
                {
                    var returnProtocol = MessageFactory.Create(MessageTypesEnum.Ack);

                    Context.SetState(new BrokerSubscribedState(Context));

                    return Result.Success(new BrokerReceiveResult(returnProtocol)
                    {
                        ContextType = new ContextAlterationRequests(typeof(PushContext))
                    });
                }
            default:
                return Result.Failure<BrokerReceiveResult>("Unexpected message type");
        }
    }
}