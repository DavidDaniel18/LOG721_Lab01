namespace Domain.Services.Receive.BrokerReceive;

public record BrokerReceiveWrapper(string RoutingKey, string Contract, byte[] Payload);