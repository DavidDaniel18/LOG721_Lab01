namespace Domain.Services.Sending.Send;

public record SendWrapper<TContract>(string SenderId, TContract Payload);