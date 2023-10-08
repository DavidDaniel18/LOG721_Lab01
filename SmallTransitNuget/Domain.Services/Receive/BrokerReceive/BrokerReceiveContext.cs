using Domain.ProtoTransit;
using Domain.Services.Receive.BrokerReceive.States;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.BrokerReceive;

internal sealed class BrokerReceiveContext : ReceiveStateHolder<BrokerReceiveContext, Protocol, BrokerReceiveResult, PublishByteWrapper>
{
    private protected override ReceiveState<BrokerReceiveContext, Protocol, BrokerReceiveResult, PublishByteWrapper> State { get; set; }

    public BrokerReceiveContext() { State = new FreeBrokerState(this); }

    internal override bool GetConnectionReady() => State is BrokerSubscribedState;
}
//{
//    private BrokerReceiveState<BrokerReceiveContext> _state;

//    public BrokerReceiveContext()
//    {
//        _state = new FreeBrokerState(this);
//    }

//    internal Result<Protocol> TryGetMessage(byte[] message)
//    {
//        Result<Protocol>? messageParsingResult;

//        if (_state == ReceiveConnectionState.Closed)
//        {
//            return Result.Success(MessageFactory.Create(MessageTypesEnum.Nack));
//        }

//        switch (_state)
//        {
//            case ReceiveConnectionState.Free:

//                messageParsingResult = TryParseMessage(message);

//                if (messageParsingResult.IsFailure()) return Result.FromFailure<Protocol>(messageParsingResult);

//                if (messageParsingResult.Content is not HandShake)
//                {
//                    _state = ReceiveConnectionState.Closing;

//                    return Result.Failure<Protocol>(new WrongMessageException());
//                }

//                _state = ReceiveConnectionState.Subscribed;

//                return Result.Success(MessageFactory.Create(MessageTypesEnum.Ack));

//            case ReceiveConnectionState.Subscribed:

//                messageParsingResult = TryParseMessage(message);

//                if (messageParsingResult.IsFailure()) return Result.FromFailure<Protocol>(messageParsingResult);

//                if (messageParsingResult.Content is Close)
//                {
//                    _state = ReceiveConnectionState.Closed;

//                    return Result.Success(MessageFactory.Create(MessageTypesEnum.Ack));
//                }

//                if (messageParsingResult.Content is not Publish)
//                {
//                    _state = ReceiveConnectionState.Closing;

//                    return Result.Failure<Protocol>(new WrongMessageException());
//                }

//                break;
//            case ReceiveConnectionState.Closing:



//                break;
//            case ReceiveConnectionState.Closed:



//                break;
//            default:
//                throw new ArgumentOutOfRangeException();
//        }



//        // sending and receiving orchestrator should have different states for sagas
//        queue.Enqueue(new PublishSagaItem<,>(messageParsingResult.Content, ));

//        var saga = new PublishSaga(new Queue<PublishSagaItem<,>>()
//    }

//    public Result SetState(State<BrokerReceiveContext> state)
//    {
//        if (state is ReceiveConnectionState receiveConnectionState)
//        {
//            _state = receiveConnectionState;
//            return Result.Success();
//        }
//        return Result.Failure("Invalid state");
//    }
//}