using ChannelEngine.Assessment.Acl;
using ChannelEngine.Assessment.Acl.ChannelEngine;
using ChannelEngine.Assessment.Application.Services;
using ChannelEngine.Assessment.Domain.Marketing.Services;
using ChannelEngine.Assessment.Infrastructure.Integrations;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = Log.ForContext<Program>();

            AppDomain.CurrentDomain.UnhandledException += (sender, exception) =>
            {
                logger.Error((Exception)exception.ExceptionObject, "UnhandledException");
            };

            logger.Information("Application started!");

            await Application(host.Services);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration)
                       .ReadFrom.Services(services)
                       .Enrich.FromLogContext();
                })
               .ConfigureAppConfiguration(cfg =>
               {
                   var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();

                   cfg.AddConfiguration(config);
               })
               .ConfigureServices(ConfigureServices);

            return hostBuilder;
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
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

        public static async Task Application(IServiceProvider serviceProvider)
        {
            var marketingService = serviceProvider.GetRequiredService<IMarketingApplicationService>();

            var warehouseService = serviceProvider.GetRequiredService<IWarehouseApplicationService>();

            var products = await marketingService.GetBestSellerProductsAsync(5, default);

            var oneOfProducts = products.FirstOrDefault();

            var result = await warehouseService.UpdateProductQuantityAsync(oneOfProducts.ProductId, 10, default);
        }
    }
}
