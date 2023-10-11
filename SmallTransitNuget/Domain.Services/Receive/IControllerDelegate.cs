namespace Domain.Services.Receive;

public interface IControllerDelegate<in TContract>
{
    Task SendToController(TContract contract);
}