using Configuration;
using SmallTransit;
using Serilog;
using SubscriberClient.Class;
using SubscriberClient.Controllers;

public class Program
{
    static void Main(string[] args)
    {

        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        var port = 1000;
        //ConfigureMassTransit(builder.Services);
        builder.Services.AddSingleton<Metrics>();
        builder.Services.AddSmallTransit(configuration =>
        {
            configuration.Host = "host.docker.internal";
            configuration.Port = port;
            configuration.AddReceiver<MessageConsumerController>($"queue{Guid.NewGuid()}", rcv =>
            {
                rcv.RoutingKey = "weather/*/temperature";
            
            });
        });
        var app = builder.Build();

       
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
       

    }

    //static void ConfigureMassTransit(IServiceCollection services)
    //{
    //    services.AddSmallTransit( config )
    //    services.AddMassTransit(config =>
    //    {
    //        config.AddConsumer<MessageConsumerController>();
    //        config.UsingRabbitMq((Context, cfg) =>
    //        {
    //            cfg.Host("rabbitmq://localhost", h =>
    //            {
    //                h.Username("guest");
    //                h.Password("guest");
    //            });
    //            cfg.ReceiveEndpoint("Message", e =>
    //            {
    //                e.UseMessageRetry(r => r.Interval(2, 100));
    //                e.Consumer<MessageConsumerController>();
    //            });
    //        });
    //    });

    //    services.AddScoped(provider => provider.GetRequiredService<IBusControl>());
    //    services.AddScoped(provider => provider.GetRequiredService<IBus>());
    //   
        

        
    //    services.AddLogging(builder =>
    //    {
    //        builder.AddConsole();
    //    });

      
    //}
}
