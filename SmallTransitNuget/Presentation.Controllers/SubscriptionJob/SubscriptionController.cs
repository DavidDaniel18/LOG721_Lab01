using Application.Services.InfrastructureInterfaces;
using Application.UseCases;
using Application.UseCases.Interfaces;
using Domain.Common;
using Domain.Services.Send.Subscribing.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation.Controllers.Dto;

namespace Presentation.Controllers.SubscriptionJob;

public sealed class SubscriptionController : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SmallTransitConfigurator _configurator;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(IServiceProvider serviceProvider, SmallTransitConfigurator configurator, ILogger<SubscriptionController> logger)
    {
        _serviceProvider = serviceProvider;
        _configurator = configurator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Parallel.ForEachAsync(_configurator.ReceiverConfigurator, stoppingToken, async (configuration, token) =>
            {
                using var scope = _serviceProvider.CreateScope();

                var genericTypeDefinition = typeof(ReceiveClient<>);

                var closedGenericType = genericTypeDefinition.MakeGenericType(configuration.ContractType);

                var receiveClient = (IReceiveClient)scope.ServiceProvider.GetRequiredService(closedGenericType);

                var networkStream = scope.ServiceProvider.GetRequiredService<INetworkStream>();

                var subscriptionResult = await receiveClient.Subscribe(
                        networkStream,
                        new SubscriptionWrapper(
                            configuration.ReceiverConfigurator.RoutingKey,
                            configuration.ContractType.Name,
                            configuration.QueueName))
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