using Application;
using Controllers.Repositories;
using Entities.Cache;
using Interfaces.Cache;
using Interfaces.Handler;
using Interfaces.Repositories;
using Interfaces.Services;
using SmallTransit;
using IRouter = Interfaces.IRouter;

namespace ConfigurationBroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigurationSetup(builder.Services);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            var webSocketOptions = new WebSocketOptions
            { 
                KeepAliveInterval = TimeSpan.FromMinutes(2),
            };

            app.UseWebSockets(webSocketOptions);
            app.MapControllers();
            app.Run();
        }

        private static void ConfigurationSetup(IServiceCollection services)
        {
            ConfigureSmallMassTransit(services);
            // Domain
            services.AddScoped<IRouter, Router>();
            // Handlers
            services.AddScoped<IPublisherHandler, PublisherHandler>();
            services.AddScoped<ISubscriptionHandler, SubscriptionHandler>();
            // Services
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IBrokerService, BrokerService>();
            services.AddScoped<IQueueService, QueueService>();
            // Repositories
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IBrokerRepository, BrokerRepository>();
            services.AddScoped<IQueueRepository, QueueRepository>();
            // Caches
            services.AddSingleton<ISubscriptionCache, SubscriptionCache>();
            services.AddSingleton<ITopicCache, TopicCache>();
            services.AddSingleton<IBrokerCache, BrokerCache>();
            services.AddSingleton<IQueueCache, QueueCache>();
        }

        private static void ConfigureSmallMassTransit(IServiceCollection services)
        {
            services.AddSmallTransit(_ => { });
            services.AddSmallTransitBroker(configurator => 
            {
                configurator.TcpPort = 32769;

                configurator.AddPublishReceiver<BrokerControllerPublisher>();
                configurator.AddSubscriptionReceiver<BrokerControllerSubscription>();
            });
        }
    }
}