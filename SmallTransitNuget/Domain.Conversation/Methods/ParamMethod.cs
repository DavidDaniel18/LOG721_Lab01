using Domain.Common;

namespace Domain.Steps.Methods;

internal sealed class ParamMethod<TParam>
{
    private readonly Func<TParam, Task<Result>> _handleAsync;

    internal ParamMethod(Func<TParam, Task<Result>> paramFunc)
    {
        _handleAsync = paramFunc;
    }

    internal async Task<Result> Execute(TParam value)
    {
        return await _handleAsync(value);
    }
}