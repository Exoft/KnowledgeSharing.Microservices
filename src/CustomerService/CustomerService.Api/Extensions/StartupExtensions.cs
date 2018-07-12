using System;
using System.Linq;
using CustomerService.Api.Filters;
using CustomerService.Infrastructure;
using CustomerService.Infrastructure.Helpers;
using CustomerService.Infrastructure.Repositories;
using CustomerService.Logic.Interfaces;
using CustomerService.MessageQueue;
using CustomerService.MessageQueue.Handlers;
using CustomerService.MessageQueue.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace CustomerService.Api.Extensions
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
            
            services.AddSingleton<MessageListener>(s =>
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
                
                return new MessageListener(connection, channel, loggerFactory);
            });

            services.AddScoped<BookingEventHandler>();
            services.AddScoped<OrderEventHandler>();

            var bookingHandler = services.BuildServiceProvider().GetService<BookingEventHandler>();
            var orderHandler = services.BuildServiceProvider().GetService<OrderEventHandler>();
            
            var listener = services.BuildServiceProvider().GetService<MessageListener>();
            listener.Subscribe(Constants.MessageQueue.BookingExchange, bookingHandler);
            listener.Subscribe(Constants.MessageQueue.OrderExchange, orderHandler);
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
            services.AddSingleton<DatabaseInitializer>();
        }

        public static void ConfigureDb(IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetValue("ConnectionString", string.Empty);
            
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly("CustomerService.Api")));

            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        public static void SeedData(IServiceCollection services)
        {
            var initializer = services.BuildServiceProvider().GetService<DatabaseInitializer>();
            initializer.Initialize();
        }
    }
}