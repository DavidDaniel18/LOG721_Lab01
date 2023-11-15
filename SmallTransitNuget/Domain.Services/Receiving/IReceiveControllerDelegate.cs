using Domain.Services.Sending.Subscribing.Dto;

namespace Domain.Services.Receiving;

public interface IReceiveControllerDelegate
{
    Task<object> SendToController(ReceiveWrapper wrapper);

    (Type Contract, Type ReturnType) GetContractType(string contractName);
}