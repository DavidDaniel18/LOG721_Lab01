
using Configuration;
using TestHost.Controllers;

namespace TestReceiver
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

            var port = 32769;

            builder.Services.AddSmallTransit(configuration =>
            {
                configuration.Host = "host.docker.internal";
                configuration.Port = port;
                configuration.AddReceiver<ReceiverController>($"queue{Guid.NewGuid()}", rcv =>
                {
                    rcv.RoutingKey = "*";
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}