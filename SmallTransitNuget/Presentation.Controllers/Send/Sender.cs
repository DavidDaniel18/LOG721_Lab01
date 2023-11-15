using Application.UseCases;
using Domain.Common.Monads;
using Domain.Services.Sending.Send;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Send;

public sealed class Sender<TContract, TResult> : ISender<TContract, TResult> where TContract : class
{
    private readonly SendClient<TContract, TResult> _sendClient;

    public Sender(SendClient<TContract, TResult> sendClient)
    {
        _sendClient = sendClient;
    }

    public async Task<Result<TResult>> Send(TContract payload, string senderId, string targetKey)
    {
        return await _sendClient.Send(new SendWrapper<TContract>(senderId, payload), targetKey);
    }
}