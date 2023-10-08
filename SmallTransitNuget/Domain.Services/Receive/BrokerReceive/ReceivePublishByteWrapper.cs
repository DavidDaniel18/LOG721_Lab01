namespace Domain.Services.Receive.BrokerReceive;

internal record PublishByteWrapper(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload)