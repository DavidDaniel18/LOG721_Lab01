using Domain.Common;

namespace Domain.Exchanges;

internal sealed class SingleStep : Step
{
    public SingleStep(Func<Task<Result>> delegatedMethod, Func<Result, Step?> nextStepSelector) : base(delegatedMethod, nextStepSelector)
    {
    }


    internal override async Task<Result> ExecuteAsync()
    {
        var result = await _delegatedMethod();

        var next = Next(result);

        if (next is null)
        {
            return Result.Failure("SingleStep should at least be followed by Close Step");
        }

        return await next.ExecuteAsync();
    }
}