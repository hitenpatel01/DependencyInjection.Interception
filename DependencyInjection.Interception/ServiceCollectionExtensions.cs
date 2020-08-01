using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependencyInjection.Interception
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProviderWithInterception(this IServiceCollection services)
        {
            services
                .AddSingleton<ProxyGenerator>()
                .AddSingleton<MethodInterceptionHandler>(provider => new MethodInterceptionHandler(provider));
            return new InterceptableServiceProvider(services.BuildServiceProvider());
        }
    }
}
