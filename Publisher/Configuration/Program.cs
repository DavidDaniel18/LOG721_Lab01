using Configuration.Controllers;
using Controlleur.Classe;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Configuration.Controllers
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
                configuration.Port = 32769;
            });

            // builder.Services.AddScoped<MessageLog721>();
            ConfigurationSetup(builder.Services);

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

        private static void ConfigurationSetup(IServiceCollection services)
        {
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(PublisherController).Assembly));

        }
    }
}