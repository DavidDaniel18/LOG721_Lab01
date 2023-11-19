using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Interfaces;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.UseCases;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.Services.Sending.Send;

namespace SmallTransit.Presentation.Controllers.Send;

public sealed class Sender<TContract, TResult> : ISender<TContract, TResult> where TContract : class
{
    private readonly SendClient<TContract, TResult> _sendClient;
    private readonly ILogger<Sender<TContract, TResult>> _logger;

    public Sender(SendClient<TContract, TResult> sendClient, ILogger<Sender<TContract, TResult>> logger)
    {
        _sendClient = sendClient;
        _logger = logger;
    }

    public async Task<Result<TResult>> Send(TContract payload, string senderId, string targetKey)
    {
        _logger.LogInformation($"Sending {typeof(TContract).GetTypeName()} to {targetKey} from {senderId}");

        return await _sendClient.Send(new SendWrapper<TContract>(senderId, payload), targetKey);
    }
}