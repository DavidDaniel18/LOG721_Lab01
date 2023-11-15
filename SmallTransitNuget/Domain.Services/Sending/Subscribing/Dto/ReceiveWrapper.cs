namespace Domain.Services.Sending.Subscribing.Dto;

public record ReceiveWrapper(string SenderId, object Payload, Type ContractType, Type ResultType);