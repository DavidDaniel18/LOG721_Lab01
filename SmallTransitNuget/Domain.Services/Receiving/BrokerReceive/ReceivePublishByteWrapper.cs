namespace Domain.Services.Receiving.BrokerReceive;

internal record ReceivePublishByteWrapper(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload);