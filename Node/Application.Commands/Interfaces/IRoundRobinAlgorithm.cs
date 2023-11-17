namespace Application.Commands.Orchestrator.Service;

public interface IRoundRobinAlgorithm
{
    public string GetNextElement();
}
