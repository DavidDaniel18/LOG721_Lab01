namespace SmallTransit.Abstractions.Broker;

public record BrokerReceiveWrapper(string RoutingKey, string Contract, byte[] Payload);