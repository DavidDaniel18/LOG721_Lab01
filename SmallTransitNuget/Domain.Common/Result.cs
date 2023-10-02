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

    public static bool IsFailure(params Result[] results)
    {
        if (results.Any() is false) return true;

        return results.Any(r => r.IsFailure());
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

    public static Result<TContent> Success<TContent>(TContent content)
    {
        return new Result<TContent>(content, null);
    }

    public static Result<TContent> Failure<TContent>(Exception exception)
    {
        return new Result<TContent>(default, exception);
    }

    public static Result<TContent> FromFailure<TContent>(Result innerResult)
    {
        return Failure<TContent>(innerResult.Exception ?? new ArgumentException("Inner result must be a failure", nameof(innerResult)));
    }

    public static Result<TContent> From<TContent>(Result<TContent> innerResult)
    {
        return innerResult;
    }
}

public record Result<TContent>(TContent? Content, Exception? Exception) : Result(Exception);