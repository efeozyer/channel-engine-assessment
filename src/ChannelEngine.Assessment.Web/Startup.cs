using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ChannelEngine.Assessment.Acl;
using ChannelEngine.Assessment.Acl.ChannelEngine;
using ChannelEngine.Assessment.Application.Services;
using ChannelEngine.Assessment.Domain.Marketing.Services;
using ChannelEngine.Assessment.Infrastructure.Integrations;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine;

namespace ChannelEngine.Assessment.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Mvc
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();

            // Integration
            services.AddHttpClient();
            services.AddIntegrationClient<ChannelEngineClient, ChannelEngineClientConfig>();

            // Adapters
            services.AddAdapter<ChannelEngineAdapterProvider>();

            // Application services
            services.AddSingleton<IMarketingApplicationService, MarketingApplicationService>();
            services.AddSingleton<IWarehouseApplicationService, WarehouseApplicationService>();

            // Domain services
            services.AddSingleton<IMarketingService, MarketingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });
        }
    }
}
