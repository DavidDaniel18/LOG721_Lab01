using Application.Services.InfrastructureInterfaces;
using Application.UseCases;
using Configuration.Dispatcher;
using Configuration.Options;
using Domain.Services.Common;
using Domain.Services.Receiving;
using Infrastructure.TcpClient;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers.BrokerReceiver;
using Presentation.Controllers.Client;
using Presentation.Controllers.Dto.Configurator;
using Presentation.Controllers.Publish;
using Presentation.Controllers.SubscriptionJob;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;
using SmallTransit.Abstractions.Receiver;

namespace Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddSmallTransit(this IServiceCollection collection, Action<IConfigurator> configurator)
    {
        Infrastructure(collection);
        Presentation(collection);
        Application(collection);

        collection.AddSingleton(typeof(IControllerDelegate<>), typeof(ControllerDispatcher<>));
        collection.AddSingleton(typeof(IReceiveControllerDelegate), typeof(ReceiveControllerDispatcher));

        collection.AddSingleton<ClientFactory>();

        var configuration = new Configurator();

        configurator?.Invoke(configuration);

        List<TargetConfiguration> targetConfigurations = new();

        targetConfigurations.AddRange(configuration.TargetPointConfigurators);

        LoadQueueConfigurations(collection, configuration, targetConfigurations);

        LoadPointConfigurations(collection, configuration);

        collection.AddSingleton(typeof(INetworkStreamCache), services =>
        {
            var cache = new NetworkStreamCache();

            foreach (var targetConfiguration in targetConfigurations)
            {
                var factory = services.GetRequiredService<ClientFactory>();

                var clientFactory = factory.RetryCreateClient(
                    targetConfiguration.Host, 
                    targetConfiguration.Port, 
                    targetConfiguration.TargetKey);

                clientFactory.ThrowIfException();

                cache.Add(clientFactory.Content!);
            }

            return cache;

        });

        collection.AddSingleton(_ => configuration);

        return collection;
    }

    private static void LoadPointConfigurations(IServiceCollection collection, Configurator configuration)
    {
        if (configuration.ReceiverPointConfigurators.Any())
        {
            collection.AddSingleton<ReceiverConnectionHandler, ReceiverFactory>();

            foreach (var receiverConfiguration in configuration.ReceiverPointConfigurators)
            {
                collection.AddScoped(receiverConfiguration.IConsumerInterface, receiverConfiguration.ReceivingController);
            }
        }
    }

    private static void LoadQueueConfigurations(IServiceCollection collection, Configurator configuration,
        List<TargetConfiguration> targetConfigurations)
    {
        if (configuration.QueueConfigurators.Any())
        {
            if (configuration.QueueConfigurators.Select(qc => qc.ReceiverConfigurator.Any()).Any())
            {
                collection.AddHostedService<SubscriptionController>();
            }

            foreach (var queueConfigurator in configuration.QueueConfigurators)
            {
                targetConfigurations.Add(queueConfigurator.TargetConfiguration);

                foreach (var receiverConfiguration in queueConfigurator.ReceiverConfigurator)
                {
                    collection.AddScoped(receiverConfiguration.IConsumerInterface,
                        receiverConfiguration.ReceivingController);
                }
            }
        }
    }

    public static IServiceCollection AddSmallTransitBroker(this IServiceCollection collection, Action<ISBrokerConfigurator> configure)
    {
        var brokerOptions = new BrokerOptions();

        configure(brokerOptions);

        collection.AddSingleton<BrokerConnectionHandler, BrokerageFactory>();

        collection.AddScoped(typeof(IConsumer<BrokerSubscriptionDto>), brokerOptions.SubscriptionReceiver);

        collection.AddScoped(typeof(IConsumer<BrokerReceiveWrapper>), brokerOptions.PublishReceive);

        collection.Configure<KestrelServerOptions>(options =>
        {
            options.ListenAnyIP(brokerOptions.TcpPort, listenOptions =>
            {
                listenOptions.UseConnectionHandler<BrokerageFactory>();
            });
        });

        collection.AddScoped<Broker>();

        collection.AddScoped<BrokerReceiver>();

        collection.AddScoped<IBrokerPushEndpoint>(serviceProvider => serviceProvider.GetRequiredService<BrokerReceiver>());

        return collection;
    }

    private static void Infrastructure(IServiceCollection collection)
    {
        collection.AddTransient<TcpBridge>();

        collection.AddSingleton<INetworkStreamCache>(serviceProvider => serviceProvider.GetRequiredService<NetworkStreamCache>());

        collection.AddTransient<ITcpBridge>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
        collection.AddTransient<IComHandler>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
    }

    private static void Presentation(IServiceCollection collection)
    {
        collection.AddScoped(typeof(IPublisher<>), typeof(Publisher<>));
    }

    private static void Application(IServiceCollection collection)
    {
        collection.AddScoped(typeof(PublishClient<>));
        collection.AddScoped(typeof(ReceiveSubscriber<>));
    }
}