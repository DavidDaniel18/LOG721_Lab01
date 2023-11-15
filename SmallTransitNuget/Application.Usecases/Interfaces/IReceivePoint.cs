using Domain.Common.Monads;

namespace Application.UseCases.Interfaces;

public interface IReceivePoint : IDisposable
{
    Task<Result> BeginListen(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}