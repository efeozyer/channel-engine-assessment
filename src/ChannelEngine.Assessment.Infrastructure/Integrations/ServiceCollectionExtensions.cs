using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ChannelEngine.Assessment.Infrastructure.Integrations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationClient<TClient, TConfig>(this IServiceCollection services)
            where TConfig : IntegrationClientConfig
        {
            var implementationType = typeof(TClient);
            var serviceType = implementationType.GetInterfaces().First();

            services.AddSingleton(serviceType, implementationType);

            services.AddSingleton(x =>
            {
                var clientConfig = Activator.CreateInstance<TConfig>();

                var appConfig = x.GetRequiredService<IConfiguration>();
                appConfig.GetSection($"Integrations:{clientConfig.Name}").Bind(clientConfig);

                return clientConfig;
            });

            return services;
        }
    }
}
