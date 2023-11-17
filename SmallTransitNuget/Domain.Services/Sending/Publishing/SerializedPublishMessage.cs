namespace SmallTransit.Domain.Services.Sending.Publishing;

internal record SerializedPublishMessage(byte[] SerializedRoutingKey, byte[] SerializedPayloadType, byte[] SerializedPayload);