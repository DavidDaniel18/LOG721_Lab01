
using Application;
using Configuration.Controllers;
using Controllers.Controllers;
using Controllers.Repositories;
using Entities.Cache;
using Interfaces;
using Interfaces.Cache;
using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Configuration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigurationSetup(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Subscribe).Assembly));
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(PublisherController).Assembly));
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriberController).Assembly));

            // Handlers
            services.AddScoped<IPublisherHandler, PublisherHandler>();
            services.AddScoped<ISubsciptionHandler, SubscriptionHandler>();

            // Services
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddScoped<IRouter, Router>();

            // Repositories
            services.AddScoped<IQueueRepository, QueueRepository>();
            services.AddScoped<IMessageRepository, MessageRespository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            // Caches
            services.AddSingleton<IQueueCache, QueueCache>();
            services.AddSingleton<IMessageCache, MessageCache>();
            services.AddSingleton<ISubscriptionCache, SubscriptionCache>();
            services.AddSingleton<IChannelCache, ChannelCache>();
            services.AddSingleton<ITopicCache, TopicCache>(); 
        }
    }
}