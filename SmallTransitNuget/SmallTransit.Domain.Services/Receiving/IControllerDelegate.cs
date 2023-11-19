namespace SmallTransit.Domain.Services.Receiving;

public interface IControllerDelegate<in TContract>
{
    Task SendToController(TContract contract);
}