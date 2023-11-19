using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.Services.Orchestrator.Sending;
using SmallTransit.Domain.Services.Sending.Send;

namespace SmallTransit.Application.UseCases;

public sealed class SendClient<TContract, TResult> : IDisposable
{
    private readonly ConcurrentDictionary<string, TargerResolver> _resolvers = new();
    private readonly INetworkStreamCache _networkStreamCache;
    private readonly ILogger<PublishClient<TContract>> _logger;
    private readonly IServiceProvider _provider;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public SendClient(INetworkStreamCache networkStreamCache, ILogger<PublishClient<TContract>> logger, IServiceProvider provider)
    {
        _networkStreamCache = networkStreamCache;
        _logger = logger;
        _provider = provider;
    }

    internal async Task<Result<TResult>> Send(SendWrapper<TContract> sendWrapper, string targetId)
    {
        var targetResolver = _resolvers.GetOrAdd(targetId, _ =>
        {
            var tcpBridge = _provider.CreateScope().ServiceProvider.GetRequiredService<ITcpBridge>();

            return new TargerResolver(tcpBridge);
        });

        var tcpBridge = targetResolver.TcpBridge;
        var publishingSendingOrchestrator = targetResolver.PublishingSendingOrchestrator;

        if (tcpBridge.IsCompleted()) return Result.Failure<TResult>("Bridging task is completed");

        if (tcpBridge.IsNotStarted())
        {
            _logger.LogInformation("Tcp bridge is not started. Starting...");

            var networkStream = _networkStreamCache.GetOrAdd(targetId, _logger);

            tcpBridge.RunAsync(networkStream.GetStream(), networkStream.GetStream(), _cancellationTokenSource);
        }
        else
        {
            _logger.LogInformation("Tcp bridge is already started");
        }

        return await publishingSendingOrchestrator.Execute(sendWrapper);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }

    private class TargerResolver
    {
        public ITcpBridge TcpBridge { get; }

        public SendOrchestrator<TContract, TResult> PublishingSendingOrchestrator { get; }

        public TargerResolver(ITcpBridge tcpBridge)
        {
            TcpBridge = tcpBridge;
            PublishingSendingOrchestrator = new SendOrchestrator<TContract, TResult>(new SendingContext(), tcpBridge); ;
        }
    }

}