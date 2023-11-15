namespace Domain.Services.Receiving.ClientReceive;

internal record ReceiveSendByteWrapper(byte[] SenderId, byte[] SerializedPayloadType, byte[] SerializedPayload);