using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.UseCases;
using SmallTransit.Application.UseCases.Interfaces;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;
using SmallTransit.Presentation.Controllers.Dto.Configurator;

namespace SmallTransit.Presentation.Controllers.SubscriptionJob;

public sealed class SubscriptionController : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Configurator _configurator;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(IServiceProvider serviceProvider, Configurator configurator, ILogger<SubscriptionController> logger)
    {
        _serviceProvider = serviceProvider;
        _configurator = configurator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Parallel.ForEachAsync(
                from configurator in _configurator.QueueConfigurators
                let targetConfiguration = configurator.TargetConfiguration
                from receiveConfiguration in configurator.ReceiverConfigurator
                select (receiveConfiguration, targetConfiguration),
                stoppingToken,
                async (configuration, token) =>
            {
                using var scope = _serviceProvider.CreateScope();

                var genericTypeDefinition = typeof(ReceiveSubscriber<>);

                var closedGenericType = genericTypeDefinition.MakeGenericType(configuration.receiveConfiguration.ContractType);

                var receiveClient = (IReceiveSubscriberClient)scope.ServiceProvider.GetRequiredService(closedGenericType);

                var networkStreamCache = scope.ServiceProvider.GetRequiredService<INetworkStreamCache>();

                var networkStream = networkStreamCache.GetOrAdd(configuration.targetConfiguration.TargetKey, _logger);

                var subscriptionResult = await receiveClient.Subscribe(
                        networkStream,
                        new SubscriptionWrapper(
                            configuration.receiveConfiguration.ReceiverConfigurator.RoutingKey,
                            configuration.receiveConfiguration.ContractType.Name,
                            configuration.receiveConfiguration.QueueName))
                    .BindAsync(receiveClient.BeginListen);

                if (subscriptionResult.IsFailure())
                {
                    _logger.LogError(subscriptionResult.Exception!.Message);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Subscription error");

            throw;
        }
    }
}