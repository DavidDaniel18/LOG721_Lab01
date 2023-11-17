namespace SmallTransit.Abstractions.Monads;

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
        Console.WriteLine(exception.Message);

        return new Result(exception);
    }

    public static Result Failure(string exceptionMessage)
    {
        Console.WriteLine(exceptionMessage);

        return Failure(new Exception(exceptionMessage));
    }

    public static Result<TContent> Success<TContent>(TContent content)
    {
        return new Result<TContent>(content, null);
    }

    public static Result<TContent> Failure<TContent>(Exception exception)
    {
        Console.WriteLine(exception.Message);

        return new Result<TContent>(default, exception);
    }

    public static Result<TContent> Failure<TContent>(string exceptionMessage)
    {
        Console.WriteLine(exceptionMessage);

        return new Result<TContent>(default, new Exception(exceptionMessage));
    }

    public static Result<TContent> FromFailure<TContent>(Result innerResult)
    {
        Console.WriteLine(innerResult.Exception?.Message);

        return Failure<TContent>(innerResult.Exception ?? new ArgumentException("Inner result must be a failure", nameof(innerResult)));
    }

    public static Result FromFailure(Result innerResult)
    {
        Console.WriteLine(innerResult.Exception?.Message);

        return Failure(innerResult.Exception ?? new ArgumentException("Inner result must be a failure", nameof(innerResult)));
    }

    public static Result From(params Result[] results)
    {
        if (IsAnyFailed(results) is { } failure) return failure;

        return Success();
    }

    public static Result Foreach<TContent>(IEnumerable<TContent> collection, Func<TContent, Result> func)
    {
        foreach (var item in collection)
        {
            var result = func(item);

            if (result.IsFailure()) return result;
        }

        return Success();
    }

    public static async Task<Result> ForeachAsync<TContent>(IEnumerable<TContent> collection, Func<TContent, Task<Result>> func)
    {
        foreach (var item in collection)
        {
            var result = await func(item);

            if (result.IsFailure()) return result;
        }

        return Success();
    }
}

public sealed record Result<TContent>(TContent? Content, Exception? Exception) : Result(Exception)
{
    public TContent GetValueOrThrow()
    {
        if (Exception is not null) throw Exception;

        return Content!;
    }

}