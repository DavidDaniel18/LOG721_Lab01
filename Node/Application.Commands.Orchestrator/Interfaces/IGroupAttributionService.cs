using Domain.Grouping;

namespace Application.Commands.Orchestrator.Interfaces;

public interface IGroupAttributionService
{
    public string GetAttributedKeyFromGroup(Group group);
}
