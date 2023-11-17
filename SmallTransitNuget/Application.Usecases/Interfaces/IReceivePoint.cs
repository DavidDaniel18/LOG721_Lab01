using SmallTransit.Abstractions.Monads;

namespace SmallTransit.Application.UseCases.Interfaces;

public interface IReceivePoint : IDisposable
{
    Task<Result> BeginListen(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}