using DependencyInjection.Interception.Sample.Interceptors;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace DependencyInjection.Interception.Sample
{
    class Program
    {
        public static void Main()
        {
            var services = new ServiceCollection();
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Trace);
            });
            services.AddMemoryCache();

            services.AddSingleton<LogInterceptor>();
            services.AddSingleton<AuthorizationInterceptor>();
            services.AddSingleton<CacheInterceptor>();
            //services.AddTransient<ICalculationService, CalculationService>();
            services.AddTransient<ICalculationService, CalculationServiceWithInterception>();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("test"), new[] { "AuthorizedUser" });

            var provider = services.BuildServiceProviderWithInterception();
            var calculationService = provider.GetService<ICalculationService>();

            var logger = provider.GetService<ILogger<Program>>();

            logger.LogInformation($"*** Executing service without interception ***");
            calculationService.Sum(1, 2);

            //logger.LogInformation($"*** Executing service with interception ***");
            //calculationService[1].Sum(3, 4);

            //Console.WriteLine("Press ANY key to quit...");
            Console.ReadKey();
        }
    }
}
