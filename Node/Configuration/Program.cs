using System.Reflection;
using Microsoft.OpenApi.Models;
using SmallTransit.Abstractions.Interfaces;
using System.Resources;
using Application.Commands.Mappers.Interfaces;
using Configuration.Properties;
using Infrastructure.Clients.Tcp;
using Infrastructure.FileHandlers.Interfaces;
using Application.Common.Interfaces;
using Application.Commands.Orchestrator.Service;
using Application.Commands.Orchestrator.Interfaces;

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

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddSmallTransit(configuration =>
            {
                configuration.Host = hostInfo.Host;
                configuration.Port = hostInfo.BrokerPort;
            });

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
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
            services.AddSingleton(_ => new ResourceManager(typeof(Resources)));

            services.AddSingleton<IDataReader, DataReader>();
        }

        private static void PresentationSetup(IServiceCollection services)
        {
            //services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(PublisherController).Assembly));
        }

        private static void InfrastructureSetup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IMessagePublisher<>), typeof(SmallTransitPublisher<>));
        }

        private static void ApplicationSetup(IServiceCollection services)
        {
            ScrutorScanForType(services, typeof(IMappingTo<,>), assemblyNames: "Application.Mapping");

            services.AddScoped<IGroupAttributionService, GroupAttributionService>();
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