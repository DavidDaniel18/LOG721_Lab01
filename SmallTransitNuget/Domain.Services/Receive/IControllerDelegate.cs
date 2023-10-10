namespace Domain.Services.Receive;

internal interface IControllerDelegate<in TContract>
{
    Task SendToController(TContract contract);
}