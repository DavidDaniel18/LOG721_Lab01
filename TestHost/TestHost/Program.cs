
using SmallTransit.Configuration;

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
                configuration.Host = "localhost";
                configuration.Port = 5672;
                configuration.AddReceiver<string>("queue", rcv =>
                {
                    rcv.PrefetchCount = 1;
                    rcv.RoutingKey = "*";
                });

                configuration.AddReceiver<string>("queue2", rcv =>
                {
                    rcv.PrefetchCount = 2;
                    rcv.RoutingKey = "john";
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