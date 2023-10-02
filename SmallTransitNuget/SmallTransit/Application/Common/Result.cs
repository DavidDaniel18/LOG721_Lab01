namespace SmallTransit.Application.Common;

public record Result(Exception? Exception)
{
    public bool IsSuccess()
    {
        return Exception is null;
    }

    public void ThrowIfException()
    {
        if (Exception is not null) throw Exception;
    }

    internal static Result Success()
    {
        return new Result(Exception: null);
    }

    internal static Result Failure(Exception exception)
    {
        return new Result(exception);
    }
}

public record Result<TContent>(TContent? Content, Exception? Exception) : Result(Exception) where TContent : class
{
    internal static Result<TContent> Success(TContent content)
    {
        return new Result<TContent>(content, null);
    }
}