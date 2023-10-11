using Configuration;
using SubscriberClient.Class;
using SubscriberClient.Controllers;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<Metrics>();

        builder.Services.AddSmallTransit(configuration =>
        {
            configuration.Host = "host.docker.internal";
            configuration.Port = 1000;
            configuration.AddReceiver<MessageConsumerController>($"queue{Guid.NewGuid()}", rcv =>
            {
                rcv.RoutingKey = Environment.GetEnvironmentVariable("RoutingKey")!;
            });
        });
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
