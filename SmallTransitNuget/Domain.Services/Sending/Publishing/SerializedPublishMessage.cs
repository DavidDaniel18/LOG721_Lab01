namespace Domain.Services.Sending.Publishing;

internal record SerializedPublishMessage(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload);