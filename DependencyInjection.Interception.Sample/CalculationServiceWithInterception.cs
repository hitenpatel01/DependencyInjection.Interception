using DependencyInjection.Interception.Sample.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Interception.Sample
{
    [Interceptable]
    [Interceptor(typeof(LogInterceptor))]
    [Interceptor(typeof(AuthorizationInterceptor))]
    public class CalculationServiceWithInterception : ICalculationService
    {
        [Interceptor(typeof(CacheInterceptor))]
        public virtual int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
