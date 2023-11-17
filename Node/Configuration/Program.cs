using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Resources;
using Application.Commands.Mappers.Interfaces;
using Infrastructure.Clients.Tcp;
using Infrastructure.FileHandlers.Interfaces;
using Application.Common.Interfaces;
using Application.Commands.Orchestrator.Service;
using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Interfaces;
using Infrastructure.FileHandlers;
using Presentation.Controllers.Tcp;
using Application.Commands.Seedwork;
using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Map.Mapping;
using Application.Commands.Map.Input;
using Application.Commands.Map.Event;
using Configuration;
using Application.Commands.Reducer.Event;
using Domain.Publicity;
using Domain.Grouping;
using Domain.Common;
using Application.Common.Cache;
using Application.Commands.Reducer;
using Application.Commands;
using ConfigurationNode.Properties;
using Application.Dtos;
using Application.Commands.Mappers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Presentation.Controllers.Rest.Controllers;

namespace ConfigurationNode
{
    public class Program
    {
        private static Action<IServiceCollection, IConfiguration> ConfigurationNode { get; set; } = ConfigurationSetup;
        private static Action<IServiceCollection> Presentation { get; set; } = PresentationSetup;
        private static Action<IServiceCollection, IConfiguration> Infrastructure { get; set; } = InfrastructureSetup;
        private static Action<IServiceCollection> Application { get; set; } = ApplicationSetup;
        private static Action<IServiceCollection> Domain { get; set; } = DomainSetup;

        public static void Main(string[] args)
        {
            var hostInfo = new HostInfo();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            Task.Delay(5000).Wait();

            ConfigureSmallTransit(builder.Services, hostInfo);

            //ConfigureSyncStore(builder.Services, hostInfo);

            ConfigureServices(builder.Services, builder.Configuration);

            builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TriggerController).Assembly));

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        //public static void ConfigureSyncStore(IServiceCollection services, IHostInfo hostInfo)
        //{
        //    services.AddSyncStore(configure =>
        //    {
        //        configure.AddPairs(cfg => hostInfo.SyncStorePairPortList.ForEach(port => cfg.AddPair("host.docker.internal", port)));
        //
        //        configure.AddStore<string, Space>();
        //        configure.AddStore<string, Group>();
        //        configure.AddStore<string, MapFinishedBool>();
        //        configure.AddStore<string, ReduceFinishedBool>();
        //    });
        //}

        public static void ConfigureSmallTransit(IServiceCollection services, IHostInfo hostInfo)
        {
            services.AddSmallTransit(/*cfg*/ configuration =>
            {
                //cfg.AddQueueConfigurator(configuration =>
                //{
                //configuration.Host("1", hostInfo.Host, hostInfo.BrokerPort);

                configuration.Host = hostInfo.Host;
                configuration.Port = hostInfo.BrokerPort;

                if (string.Equals(hostInfo.NodeType, "map"))
                {
                    configuration.AddReceiver<MapController>($"worker.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                    {
                        rcv.RoutingKey = hostInfo.MapRoutingKey;
                    });

                    if (hostInfo.IsMaster)
                    {
                        configuration.AddReceiver<InputEventController>($"worker.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.InputRoutingKey;
                        });

                        configuration.AddReceiver<MapFinishedEventController>($"orchestrator.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.MapFinishedEventRoutingKey;
                        });

                        configuration.AddReceiver<ShuffleController>($"orchestrator.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.MapShuffleRoutingKey;
                        });
                    }
                }

                if (string.Equals(hostInfo.NodeType, "reduce"))
                {
                    configuration.AddReceiver<ReduceController>($"worker.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                    {
                        rcv.RoutingKey = hostInfo.ReduceRoutingKey;
                    });

                    if (hostInfo.IsMaster)
                    {
                        configuration.AddReceiver<ReduceFinishedEventController>($"orchestrator.queue-{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.ReduceFinishedEventRoutingKey;
                        });
                    }
                }
                //});
            });
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration builderConfiguration)
        {
            Domain(services);
            Application(services);
            Infrastructure(services, builderConfiguration);
            Presentation(services);
            ConfigurationNode(services, builderConfiguration);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Node", Version = "v1" });
            });
        }

        private static void ConfigurationSetup(IServiceCollection services, IConfiguration builderConfiguration)
        {
            //services.AddSingleton(_ => new ResourceManager(typeof(Resources)));

            //ResourceManager resourceManager = new ResourceManager("Resources", typeof(Program).Assembly);

            services.AddSingleton<IDataReader, DataReader>();
        }

        private static void PresentationSetup(IServiceCollection services)
        {
            // Map Section

            services.AddScoped<ICommandHandler<InputCommand>, InputHandler>();

            services.AddScoped<ICommandHandler<MapCommand>, MapHandler>();

            services.AddScoped<ICommandHandler<MapFinishedEvent>, MapFinishedEventHandler>();

            services.AddScoped<ICommandHandler<Shuffle>, ShuffleHander>();

            // Reduce Section

            services.AddScoped<ICommandHandler<Reduce>, ReduceHandler>();

            services.AddScoped<ICommandHandler<ReduceFinishedEvent>, ReduceFinishedEventHandler>();

            // other

            services.AddScoped<IGroupAttributionService, GroupAttributionService>();
        }

        private static void InfrastructureSetup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IMessagePublisher<>), typeof(SmallTransitPublisher<>));

            services.AddScoped<ICsvHandler, CsvHandler>();

            services.AddScoped<IHostInfo, HostInfo>();

            // todo: dynamically allocate cache depends on role.
            services.AddSingleton<ISingletonCache<Group>, SingletonCache<Group>>();
            services.AddSingleton<ISingletonCache<Space>, SingletonCache<Space>>();
            services.AddSingleton<ISingletonCache<MapFinishedBool>, SingletonCache<MapFinishedBool>>();
            services.AddSingleton<ISingletonCache<ReduceFinishedBool>, SingletonCache<ReduceFinishedBool>>();
        }

        private static void ApplicationSetup(IServiceCollection services)
        {
            //ScrutorScanForType(services, typeof(IMappingTo<,>), assemblyNames: "Application.Mapping");

            services.AddScoped<IMappingTo<GroupDto, Group>, GroupMapper>();
            services.AddScoped<IMappingTo<SpaceDto, Space>, SpaceMapper>();

            services.AddScoped<IGroupAttributionService, GroupAttributionService>();

            services.AddScoped<IResultService, ResultService>();
        }

        private static void DomainSetup(IServiceCollection services)
        {
        }

        private static void ScrutorScanForType(IServiceCollection services, Type type,
            ServiceLifetime lifetime = ServiceLifetime.Scoped, params string[] assemblyNames)
        {
            services.Scan(selector =>
            {
                selector.FromAssemblies(assemblyNames.Select(Assembly.Load))
                    .AddClasses(filter => filter.AssignableTo(type))
                    .AsImplementedInterfaces()
                    .WithLifetime(lifetime);
            });
        }
    }
}