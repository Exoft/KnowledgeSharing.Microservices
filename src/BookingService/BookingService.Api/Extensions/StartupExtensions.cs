using System;
using BookingService.CustomerServiceApi.Interfaces;
using BookingService.CustomerServiceApi.Services;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Repositories;
using BookingService.Logic.Interfaces;
using BookingService.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace BookingService.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureEventBus(IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            
            var host = configuration.GetValue("RabbitMq:Host", string.Empty);
            if (!int.TryParse(configuration.GetValue("RabbitMq:Port", string.Empty), out var port))
            {
                port = 5672;
            }
            
            services.AddSingleton<RabbitMqClient>(s =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = host,
                    Port = port,
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(15)
                };
                
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();
                
                return new RabbitMqClient(connection, channel, loggerFactory);
            });
            
        }

        public static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
        
        public static void AddServices(IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var url = configuration.GetValue<string>("CustomerServiceApiUrl");
            
            services.AddScoped<IBookingService, Logic.Services.BookingService>();
            services.AddSingleton<ICustomerServiceApiClient, CustomerServiceApiClient>(opt =>
                new CustomerServiceApiClient(url));
            services.AddScoped<BookingRepository>();
        }

        public static void ConfigureDb(IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetValue("ConnectionString", string.Empty);

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly("BookingService.Api")));
            
            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}