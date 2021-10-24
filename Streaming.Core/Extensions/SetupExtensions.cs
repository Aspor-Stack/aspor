using Microsoft.Extensions.DependencyInjection;
using Aspor.Streaming.Core.Bus;
using System;

namespace Aspor.Streaming.Core.Extensions
{

    public static class SetupExtensions
    {

        public static IServiceCollection AddAsporStreaming(this IServiceCollection services)
        {
            services.AddSingleton<IStreamProvider>(new DefaultStreamProvider());
            return services;
        }

        public static IServiceCollection AddAsporStreaming(this IServiceCollection services, Action<IStreamProvider> builder)
        {
            IStreamProvider provider = new DefaultStreamProvider();
            builder.Invoke(provider);
            services.AddSingleton<IStreamProvider>(provider);
            return services;
        }

        public static IStreamProvider UseMemoryBus(this IStreamProvider provider)
        {
            provider.UseBus(new MemoryStreamBus());
            return provider;
        }
    }

}
