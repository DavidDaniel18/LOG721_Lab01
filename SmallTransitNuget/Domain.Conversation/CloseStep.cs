using Domain.Common;

namespace Domain.Exchanges;

internal sealed class CloseStep : Step
{
    public CloseStep(Func<Task<Result>> delegatedMethod) : base(delegatedMethod, _ => default)
    {
    }


    internal override async Task<Result> ExecuteAsync()
    {
        return await _delegatedMethod();
    }
}