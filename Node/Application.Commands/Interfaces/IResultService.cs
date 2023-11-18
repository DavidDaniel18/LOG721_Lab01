namespace Application.Commands.Interfaces;

public interface IResultService
{
    public void DisplayResults();
    public bool HasMoreIterations();
    public bool HasFinishedCollectedResults();
    public void IncrementIteration();
    public void ReceiveResult(string groupId, double delta);
}
