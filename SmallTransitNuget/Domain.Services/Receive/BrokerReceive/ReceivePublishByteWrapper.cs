namespace Domain.Services.Receive.BrokerReceive;

internal record ReceivePublishByteWrapper(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload);