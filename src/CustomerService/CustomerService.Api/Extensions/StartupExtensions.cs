using System;
using CustomerService.Api.Filters;
using CustomerService.Infrastructure;
using CustomerService.Infrastructure.Repositories;
using CustomerService.Logic.Interfaces;
using CustomerService.MessageQueue;
using CustomerService.MessageQueue.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace CustomerService.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureEventBus(IServiceCollection services)
        {
            services.AddSingleton<MessageListener>(s =>
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
   
                return new MessageListener(connection, channel);
            });
            
            var listener = services.BuildServiceProvider().GetService<MessageListener>();
            listener.Subscribe(Constants.MessageQueue.BookingExchange);
            listener.Subscribe(Constants.MessageQueue.OrderExchange);
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
            services.AddScoped<ICustomerService, Logic.Services.CustomerService>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<ApplicationExceptionFilter>();
        }

        public static void ConfigureDb(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql("Host=localhost;Database=customer_service;Username=postgres;Password=postgres",
                    b => b.MigrationsAssembly("CustomerService.Api")));

            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}