using Domain.Publicity;

namespace Application.Commands.Orchestrator.Interfaces;

public interface IGroupAttributionService
{
    public string GetAttributedKeyFromSpace(Space space);
}
