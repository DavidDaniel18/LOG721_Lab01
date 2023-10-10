namespace Domain.Common;

public record Result(Exception? Exception)
{
    public bool IsSuccess()
    {
        return Exception is null;
    }

    public bool IsFailure()
    {
        return IsSuccess() is false;
    }

    private static Result? IsAnyFailed(Result[] results)
    {
        return results.FirstOrDefault(r => r.IsFailure());
    }

    public void ThrowIfException()
    {
        if (Exception is not null) throw Exception;
    }

    public static Result Success()
    {
        return new Result(Exception: null);
    }

    public static Result Failure(Exception exception)
    {
        return new Result(exception);
    }

    public static Result Failure(string exceptionMessage)
    {
        return Failure(new Exception(exceptionMessage));
    }

    public static Result<TContent> Success<TContent>(TContent content)
    {
        return new Result<TContent>(content, null);
    }

    public static Result<TContent> Failure<TContent>(Exception exception)
    {
        return new Result<TContent>(default, exception);
    }

    public static Result<TContent> Failure<TContent>(string exceptionMessage)
    {
        return new Result<TContent>(default, new Exception(exceptionMessage));
    }

    public static Result<TContent> FromFailure<TContent>(Result innerResult)
    {
        return Failure<TContent>(innerResult.Exception ?? new ArgumentException("Inner result must be a failure", nameof(innerResult)));
    }

    public static Result FromFailure(Result innerResult)
    {
        return Failure(innerResult.Exception ?? new ArgumentException("Inner result must be a failure", nameof(innerResult)));
    }

    public static Result From(params Result[] results)
    {
        if (IsAnyFailed(results) is {} failure) return failure;

        return Success();
    }
}

public static class ResultMonadExtensions
{
    public static Result<TResult> Bind<T, TResult>(
        this Result<T> result,
        Func<T, Result<TResult>> bindFunc)
    {
        if (result.IsFailure()) return Result.FromFailure<TResult>(result);

        return bindFunc(result.Content!);
    }

    public static Result Bind(
        this Result result,
        Func<Result> bindFunc)
    {
        if (result.IsFailure()) return Result.FromFailure(result);

        return bindFunc();
    }

    public static Result<T> Bind<T>(
        this Result result,
        Func<Result<T>> bindFunc)
    {
        if (result.IsFailure()) return Result.FromFailure<T>(result);

        return bindFunc();
    }

    public static Result Bind<T>(
        this Result<T> result,
        Func<T, Result> bindFunc)
    {
        if (result.IsFailure()) return Result.FromFailure(result);

        return bindFunc(result.Content!);
    }

    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<Result<TResult>>> bindFuncAsync)
    {
        if (result.IsFailure()) return Result.FromFailure<TResult>(result);

        return await bindFuncAsync(result.Content!);
    }

    public static async Task<Result> BindAsync(
        this Task<Result> resultTask,
        Func<Task<Result>> bindFuncAsync)
    {
        var result = await resultTask;

        if (result.IsFailure()) return Result.FromFailure(result);

        return await bindFuncAsync();
    }

    public static async Task<Result<TResult>> BindAsync<TResult>(
        this Task<Result> resultTask,
        Func<Result<TResult>> bindFuncAsync)
    {
        var result = await resultTask;

        if (result.IsFailure()) return Result.FromFailure<TResult>(result);

        return bindFuncAsync();
    }

    public static async Task<Result> BindAsync<T>(
        this Result<T> result,
        Func<T, Task<Result>> bindFuncAsync)
    {
        if (result.IsFailure()) return Result.FromFailure(result);

        return await bindFuncAsync(result.Content!);
    }


    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TResult>>> bindFunc)
    {
        var result = await resultTask;

        if (result.IsFailure()) return Result.FromFailure<TResult>(result);

        return await bindFunc(result.Content!);
    }

    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, Result<TResult>> bindFunc)
    {
        var result = await resultTask;

        if (result.IsFailure()) return Result.FromFailure<TResult>(result);

        return bindFunc(result.Content!);
    }
}

public record Result<TContent>(TContent? Content, Exception? Exception) : Result(Exception);