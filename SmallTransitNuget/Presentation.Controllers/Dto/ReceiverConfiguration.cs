namespace Presentation.Controllers.Dto;

public sealed class ReceiverConfiguration
{
    public Type ContractType { get; }

    public Type IConsumerInterface { get; }

    public Type ReceivingController { get; }

    internal string QueueName { get; }

    internal ReceiverConfigurator ReceiverConfigurator { get; }

    public ReceiverConfiguration(Type contractType, Type iConsumerInterface, Type receivingController, string queueName, ReceiverConfigurator receiverConfigurator)
    {
        ContractType = contractType;
        IConsumerInterface = iConsumerInterface;
        ReceivingController = receivingController;
        QueueName = queueName;
        ReceiverConfigurator = receiverConfigurator;
    }
}