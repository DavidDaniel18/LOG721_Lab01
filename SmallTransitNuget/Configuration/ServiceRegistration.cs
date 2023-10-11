using Application.Services.InfrastructureInterfaces;
using Application.UseCases;
using Configuration.Dispatcher;
using Configuration.Options;
using Domain.Services.Common;
using Domain.Services.Receive;
using Infrastructure.TcpClient;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers.BrokerReceiver;
using Presentation.Controllers.Dto;
using Presentation.Controllers.Publish;
using Presentation.Controllers.SubscriptionJob;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;

namespace Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddSmallTransit(this IServiceCollection collection, Action<ISmallTransitConfigurator>? configure = null)
    {
        var configuration = new SmallTransitConfigurator();

        configure?.Invoke(configuration);

        foreach (var receiverConfiguration in configuration.ReceiverConfigurator)
        {
            collection.AddScoped(receiverConfiguration.IConsumerInterface, receiverConfiguration.ReceivingController);
        }

        if (configuration.ReceiverConfigurator.Any())
        {
            collection.AddHostedService<SubscriptionController>();

            collection.AddSingleton(_ => configuration);
        }

        collection.AddSingleton(typeof(IControllerDelegate<>), typeof(ControllerDispatcher<>));

        collection.AddSingleton<ClientFactory>();

        collection.AddScoped(services =>
        {
            var factory = services.GetRequiredService<ClientFactory>();

            var clientFactory = factory.RetryCreateClient(configuration.Host, configuration.Port);

            clientFactory.ThrowIfException();

            return clientFactory.Content!;
        });

        Infrastructure(collection);
        Presentation(collection);
        Application(collection);

        return collection;
    }

    private static void Infrastructure(IServiceCollection collection)
    {
        collection.AddScoped<TcpBridge>();

        collection.AddScoped<ITcpBridge>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
        collection.AddScoped<IComHandler>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
    }

    private static void Presentation(IServiceCollection collection)
    {
        collection.AddScoped(typeof(IPublisher<>), typeof(Publisher<>));

        collection.AddSingleton<BrokerConnectionHandler, BrokerageFactory>();
    }

    private static void Application(IServiceCollection collection)
    {
        collection.AddScoped(typeof(PublishClient<>));
        collection.AddScoped(typeof(ReceiveClient<>));
    }

    public static IServiceCollection AddSmallTransitBroker(this IServiceCollection collection, Action<ISmallTransitBrokerConfigurator> configure)
    {
        var brokerOptions = new BrokerOptions();

        configure(brokerOptions);

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
}