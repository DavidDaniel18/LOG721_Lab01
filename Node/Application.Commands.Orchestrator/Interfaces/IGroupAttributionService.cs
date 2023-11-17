using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Interfaces;

public interface IGroupAttributionService
{
    public string GetAttributedKeyFromGroup(Group group);
}
