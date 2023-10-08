using Domain.Common;

namespace Domain.Steps.Methods;

internal sealed class ResultMethod
{
    private readonly Func<Task<Result>> _handleAsync;

    public ResultMethod(Func<Task<Result>> handleAsync)
    {
        _handleAsync = handleAsync;
    }

    internal async Task<Result> Execute()
    {
        return await _handleAsync();
    }
}