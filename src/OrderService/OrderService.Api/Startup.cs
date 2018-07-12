using System.IO;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Api.Extensions;
using OrderService.Api.Filters;

namespace OrderService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional:true)
                .Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            ConfigureExtensions(services);

            services.AddMvc(options => { options.Filters.Add<ApplicationExceptionFilter>(); });
            services.AddCorrelationId();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            StartupExtensions.ConfigureSwagger(app);
        
            app.UseCorrelationId(new CorrelationIdOptions {UseGuidForCorrelationId = true});
            app.UseMvc();
            app.UseStaticFiles();
        }
        
        #region Helpers

        private static void ConfigureExtensions(IServiceCollection services)
        {
            StartupExtensions.AddServices(services);
            StartupExtensions.ConfigureDb(services);
            StartupExtensions.AddSwagger(services);
            StartupExtensions.ConfigureEventBus(services);
            StartupExtensions.ConfigureMediator(services);
        }

        #endregion
    }
}