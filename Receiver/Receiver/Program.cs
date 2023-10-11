
using Configuration;
using Receiver.Controllers;
using Receiver.Entities;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<Metrics>();

            builder.Services.AddSmallTransit(configuration =>
            {
                configuration.Host = "host.docker.internal";
                configuration.Port = 32769;
                configuration.AddReceiver<MessageConsumerController>($"queue{Guid.NewGuid()}", rcv =>
                {
                    rcv.RoutingKey = Environment.GetEnvironmentVariable("RoutingKey") ?? "*";
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}