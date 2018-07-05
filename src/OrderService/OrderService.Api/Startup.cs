using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Api.Extensions;
using OrderService.Api.Filters;

namespace OrderService.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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