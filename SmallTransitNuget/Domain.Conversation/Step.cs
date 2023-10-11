using Domain.Common;

namespace Domain.Exchanges;

internal abstract class Step
{
    private protected readonly Func<Task<Result>> _delegatedMethod;
    private readonly Func<Result, Step?> _nextStepSelector;

    internal Step(Func<Task<Result>> delegatedMethod, Func<Result, Step?> nextStepSelector)
    {
        _delegatedMethod = delegatedMethod;
        _nextStepSelector = nextStepSelector;
    }

    internal Step? Next(Result result) => _nextStepSelector(result);

    internal abstract Task<Result> ExecuteAsync();
}