namespace Domain.Services.Send.Publishing;

internal record SerializedPublishMessage(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload);