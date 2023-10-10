namespace Configuration.Options;

internal sealed class ReceiverConfiguration
{
    internal Type ContractType { get; }

    internal string QueueName { get; }

    internal ReceiverConfigurator ReceiverConfigurator { get; }

    public ReceiverConfiguration(Type contractType, string queueName, ReceiverConfigurator receiverConfigurator)
    {
        ContractType = contractType;
        QueueName = queueName;
        ReceiverConfigurator = receiverConfigurator;
    }
}