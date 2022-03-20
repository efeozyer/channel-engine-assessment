using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChannelEngine.Assessment.Acl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapter<TProvider>(this IServiceCollection services)
            where TProvider : IAdapterProvider
        {
            var instance = Activator.CreateInstance<TProvider>();
            foreach(var adapter in instance.GetAll())
            {
                services.AddSingleton(adapter.Key, adapter.Value);
            }

            return services;
        }
    }
}
