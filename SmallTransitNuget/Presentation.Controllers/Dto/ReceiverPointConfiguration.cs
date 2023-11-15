namespace Presentation.Controllers.Dto;

public sealed class ReceiverPointConfiguration
{
    public Type ContractType { get; }

    public Type ResultType { get; }

    public Type IConsumerInterface { get; }

    public Type ReceivingController { get; }

    public ReceiverPointConfiguration(Type contractType, Type resultType, Type iConsumerInterface, Type receivingController)
    {
        ResultType = resultType;
        ContractType = contractType;
        IConsumerInterface = iConsumerInterface;
        ReceivingController = receivingController;
    }
}