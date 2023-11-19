namespace Application.Commands.Interfaces;

public interface IResultService
{
    public Task DisplayResults();
    public Task<bool> HasMoreIterations();
    public Task<bool> HasFinishedCollectedResults();
    public Task IncrementIteration();
    public Task ReceiveResult(string groupId, double delta);
}
