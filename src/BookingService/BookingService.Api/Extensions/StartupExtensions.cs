using System;
using BookingService.CustomerServiceApi.Interfaces;
using BookingService.CustomerServiceApi.Services;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Repositories;
using BookingService.Logic.Interfaces;
using BookingService.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace BookingService.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureEventBus(IServiceCollection services)
        {
            services.AddSingleton<RabbitMqClient>(s =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(15)
                };
                
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                
                return new RabbitMqClient(connection, channel);
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
            services.AddScoped<IBookingService, Logic.Services.BookingService>();
            services.AddSingleton<ICustomerServiceApiClient, CustomerServiceApiClient>();
            services.AddScoped<BookingRepository>();
        }

        public static void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql("Host=localhost;Database=booking_service;Username=postgres;Password=postgres",
                    b => b.MigrationsAssembly("BookingService.Api")));
            
            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}