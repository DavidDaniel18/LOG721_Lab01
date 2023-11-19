using System.Reflection;
using Application.Commands;
using Application.Commands.Interfaces;
using Application.Commands.Map.Event;
using Application.Commands.Map.Input;
using Application.Commands.Map.Map;
using Application.Commands.Mappers;
using Application.Commands.Mappers.Interfaces;
using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Orchestrator.Service;
using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Reducer.Event;
using Application.Commands.Reducer.Reduce;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Grouping;
using Domain.Publicity;
using Infrastructure.Clients.Tcp;
using Infrastructure.FileHandlers;
using Infrastructure.FileHandlers.Interfaces;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
using Presentation.Controllers.Rest.Controllers;
using Presentation.Controllers.Tcp;
using SyncStore;

namespace Configuration
{
    public class Program
    {
        private static Action<IServiceCollection, IConfiguration> Configuration { get; set; } = ConfigurationSetup;
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

            ConfigureSyncStore(builder.Services, hostInfo);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static void ConfigureSyncStore(IServiceCollection services, IHostInfo hostInfo)
        {
            services.AddSyncStore(configure =>
            {
                configure.ExposedPort = 3000;

                configure.AddPairs(cfg => hostInfo.SyncStorePairPortList.ForEach(port => cfg.AddPair("host.docker.internal", port)));

                configure.AddStore<string, Space>();
                configure.AddStore<string, Group>();
                configure.AddStore<string, MapFinishedBool>();
                configure.AddStore<string, ReduceFinishedBool>();
            },
            queueConfigurator =>
            {
                queueConfigurator.Host("Broker1", hostInfo.Host, hostInfo.BrokerPort);

                if (string.Equals(hostInfo.NodeType, "map"))
                {
                    queueConfigurator.AddReceiver<MapController>($"worker.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                    {
                        rcv.RoutingKey = hostInfo.MapRoutingKey;
                    });

                    if (hostInfo.IsMaster)
                    {
                        queueConfigurator.AddReceiver<InputEventController>($"worker.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.InputRoutingKey;
                        });

                        queueConfigurator.AddReceiver<MapFinishedEventController>($"orchestrator.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.MapFinishedEventRoutingKey;
                        });

                        queueConfigurator.AddReceiver<ShuffleController>($"orchestrator.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.MapShuffleRoutingKey;
                        });
                    }
                }

                if (string.Equals(hostInfo.NodeType, "reduce"))
                {
                    queueConfigurator.AddReceiver<ReduceController>($"worker.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                    {
                        rcv.RoutingKey = hostInfo.ReduceRoutingKey;
                    });

                    if (hostInfo.IsMaster)
                    {
                        queueConfigurator.AddReceiver<ReduceFinishedEventController>($"orchestrator.queue.{hostInfo.NodeName}.{Guid.NewGuid()}", rcv =>
                        {
                            rcv.RoutingKey = hostInfo.ReduceFinishedEventRoutingKey;
                        });
                    }
                }
            });
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration builderConfiguration)
        {
            Domain(services);
            Application(services);
            Infrastructure(services, builderConfiguration);
            Presentation(services);
            Configuration(services, builderConfiguration);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Node", Version = "v1" });
            });
        }

        private static void ConfigurationSetup(IServiceCollection services, IConfiguration builderConfiguration)
        {
            //services.AddSingleton(_ => new ResourceManager(typeof(Resources)));

            services.AddSingleton<IDataReader, DataReader>();
        }

        private static void PresentationSetup(IServiceCollection services)
        {
            // Map Section

            services.AddScoped<ICommandHandler<InputCommand>, InputHandler>();

            services.AddScoped<ICommandHandler<MapCommand>, MapHandler>();

            services.AddScoped<ICommandHandler<MapFinishedEvent>, MapFinishedEventHandler>();

            services.AddScoped<ICommandHandler<Shuffle>, ShuffleHandler>();

            // Reduce Section

            services.AddScoped<ICommandHandler<Reduce>, ReduceHandler>();

            services.AddScoped<ICommandHandler<ReduceFinishedEvent>, ReduceFinishedEventHandler>();

            // other

            services.AddScoped<IGroupAttributionService, GroupAttributionService>();

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TriggerController).Assembly));
        }

        private static void InfrastructureSetup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IMessagePublisher<>), typeof(SmallTransitPublisher<>));

            services.AddScoped<ICsvHandler, CsvHandler>();

            services.AddScoped<IHostInfo, HostInfo>();
        }

        private static void ApplicationSetup(IServiceCollection services)
        {
            services.AddScoped<IMappingTo<GroupDto, Group>, GroupMapper>();
            services.AddScoped<IMappingTo<DataDto, Space>, SpaceMapper>();

            services.AddScoped<IGroupAttributionService, GroupAttributionService>();

            services.AddScoped<IResultService, ResultService>();
        }

        private static void DomainSetup(IServiceCollection services)
        {
        }
    }
}