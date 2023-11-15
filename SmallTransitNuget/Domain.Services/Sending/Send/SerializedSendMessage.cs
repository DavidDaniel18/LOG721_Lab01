namespace Domain.Services.Sending.Send;

internal record SerializedSendMessage(byte[] SenderId, byte[] SerializedPayloadType, byte[] SerializedPayload);