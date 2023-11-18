using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;
using SmallTransit.Abstractions.Receiver;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.UseCases;
using SmallTransit.Dispatcher;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Infrastructure.TcpClient;
using SmallTransit.Options;
using SmallTransit.Presentation.Controllers.BrokerReceiver;
using SmallTransit.Presentation.Controllers.Client;
using SmallTransit.Presentation.Controllers.Dto.Configurator;
using SmallTransit.Presentation.Controllers.Publish;
using SmallTransit.Presentation.Controllers.Send;
using SmallTransit.Presentation.Controllers.SubscriptionJob;

namespace SmallTransit;

public static class ServiceRegistration
{
    public static IServiceCollection AddSmallTransit(this IServiceCollection collection, Action<IConfigurator> configurator)
    {
        Infrastructure(collection);
        Presentation(collection);
        Application(collection);

        collection.AddSingleton(typeof(IControllerDelegate<>), typeof(ControllerDispatcher<>));
        collection.AddSingleton(typeof(IReceiveControllerDelegate), typeof(ReceiveControllerDispatcher));

        var configuration = new Configurator();

        configurator?.Invoke(configuration);

        collection.AddSingleton<ClientFactory>();

        List<TargetConfiguration> targetConfigurations = new();

        targetConfigurations.AddRange(configuration.TargetPointConfigurators);
        targetConfigurations.AddRange(configuration.QueueConfigurators.Select(queue => queue.TargetConfiguration).OfType<TargetConfiguration>().ToList());

        targetConfigurations = targetConfigurations.DistinctBy(targetConfiguration => targetConfiguration.TargetKey).ToList();

        LoadQueueConfigurations(collection, configuration);

        LoadPointConfigurations(collection, configuration);

        collection.AddTransient(typeof(INetworkStreamCache), services =>
        {
            var factory = services.GetRequiredService<ClientFactory>();

            var keyValuePairs = targetConfigurations
                .ConvertAll(target => new KeyValuePair<string, ITargetConfiguration>(target.TargetKey, target));

            keyValuePairs = keyValuePairs.DistinctBy(pair => pair.Key).ToList();

            foreach (var pair in keyValuePairs)
            {
                Console.WriteLine($"pair key:{pair.Key}, pair value host: {pair.Value.Host}, pair value port: {pair.Value.Port}");
            }

            var cache = new NetworkStreamCache(new ConcurrentDictionary<string, ITargetConfiguration>(), factory);

            return cache;
        });

        collection.AddSingleton(_ => configuration);

        return collection;
    }

    private static void LoadPointConfigurations(IServiceCollection collection, Configurator configuration)
    {
        if (configuration.ReceiverPointConfigurators.Any())
        {
            collection.Configure<KestrelServerOptions>(options =>
            {
                options.ListenAnyIP(configuration.ExposedPortPoint, listenOptions =>
                {
                    listenOptions.UseConnectionHandler<ReceiverFactory>();
                });

                options.ListenAnyIP(80);
            });

            collection.AddSingleton<ReceiverConnectionHandler, ReceiverFactory>();

            foreach (var receiverConfiguration in configuration.ReceiverPointConfigurators)
            {
                collection.AddScoped(receiverConfiguration.IConsumerInterface, receiverConfiguration.ReceivingController);

                var genericISender = MakeGeneric((receiverConfiguration.ContractType, receiverConfiguration.ResultType), typeof(ISender<,>));
                var genericSender = MakeGeneric((receiverConfiguration.ContractType, receiverConfiguration.ResultType), typeof(Sender<,>));

                collection.AddScoped(genericISender, genericSender);
            }
        }
    }

    private static void LoadQueueConfigurations(IServiceCollection collection, Configurator configuration)
    {
        if (configuration.QueueConfigurators.Any())
        {
            if (configuration.QueueConfigurators.Select(qc => qc.ReceiverConfigurator.Any()).Any())
            {
                collection.AddHostedService<SubscriptionController>();
            }

            foreach (var queueConfigurator in configuration.QueueConfigurators)
            {
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

        collection.AddTransient<ITcpBridge>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
        collection.AddTransient<IComHandler>(serviceProvider => serviceProvider.GetRequiredService<TcpBridge>());
    }

    private static void Presentation(IServiceCollection collection)
    {
        collection.AddScoped(typeof(IPublisher<>), typeof(Publisher<>));
        collection.AddScoped(typeof(Receiver));
    }

    private static void Application(IServiceCollection collection)
    {
        collection.AddScoped(typeof(PublishClient<>));
        collection.AddScoped(typeof(ReceiveSubscriber<>));
        collection.AddScoped(typeof(SendClient<,>));
        collection.AddScoped(typeof(ReceivePoint));
    }

    private static Type MakeGeneric((Type TContract, Type TResult) types, Type genericTypeDefinition)
    {
        return genericTypeDefinition.MakeGenericType(types.TContract, types.TResult);
    }
}