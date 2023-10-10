
using Configuration;

namespace TestHost
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

            builder.Services.AddSmallTransit(configuration =>
            {
                configuration.Host = "host.docker.internal";
                configuration.Port = 3450;
                configuration.AddReceiver<MqController>("queue", rcv =>
                {
                    rcv.RoutingKey = "*";
                });
            });

            builder.Services.AddSmallTransitBroker(3450);

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