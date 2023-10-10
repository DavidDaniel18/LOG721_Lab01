using Application.Common.Interfaces;
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
using Presentation.Controllers.Publish;

namespace Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddSmallTransit(this IServiceCollection collection, Action<SmallTransitConfigurator>? configure = null)
    {
        var configuration = new SmallTransitConfigurator();

        configure?.Invoke(configuration);

        foreach (var receiverConfiguration in configuration.ReceiverConfigurator)
        {
            collection.AddScoped(receiverConfiguration.ContractType);
        }

        collection.AddSingleton(typeof(IControllerDelegate<>), typeof(ControllerDispatcher<>));

        Infrastructure(collection);
        Presentation(collection);
        Application(collection);

        return collection;
    }

    private static IServiceCollection Infrastructure(IServiceCollection collection)
    {
        collection.AddScoped<ITcpBridge, TcpBridge>();
        collection.AddScoped<IComHandler, TcpBridge>();

        return collection;
    }

    private static IServiceCollection Presentation(IServiceCollection collection)
    {
        //ScrutorScanForType(collection, typeof(IConsumer<>));
        collection.AddScoped(typeof(IPublisher<>), typeof(Publisher<>));

        return collection;
    }

    private static IServiceCollection Application(IServiceCollection collection)
    {
        collection.AddScoped<Broker>();
        collection.AddScoped(typeof(PublishClient<>));
        collection.AddScoped(typeof(ReceiveClient<>));

        return collection;
    }

    public static IServiceCollection AddSmallTransitBroker(this IServiceCollection collection, int tcpPort)
    {
        collection.Configure<KestrelServerOptions>(options =>
        {
            options.ListenAnyIP(tcpPort, listenOptions => listenOptions.UseConnectionHandler<BrokerReceiver>());
        });

        collection.AddScoped<BrokerReceiver>();

        return collection;
    }

    private static void ScrutorScanForType(IServiceCollection services, Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Scan(selector =>
        {
            selector.FromAssemblies()
                .AddClasses(filter => filter.AssignableTo(type))
                .AsImplementedInterfaces()
                .WithLifetime(lifetime);
        });
    }
}