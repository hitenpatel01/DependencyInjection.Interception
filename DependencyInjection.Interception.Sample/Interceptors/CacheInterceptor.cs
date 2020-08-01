using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DependencyInjection.Interception.Sample.Interceptors
{
    public class CacheInterceptor : Interceptor
    {
        private readonly ILogger<CacheInterceptor> _logger;
        private readonly IMemoryCache _cache;

        public CacheInterceptor(ILogger<CacheInterceptor> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public override void PreInterceptor(InterceptionContext context)
        {
            var cacheKey = String.Join(",", context.Arguments);
            object result;
            if (false == _cache.TryGetValue(cacheKey, out result))
            {
                _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Cache miss for {cacheKey}");
            }
            else
            {
                _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Cache hit for {cacheKey}");
                context.ReturnValue = result;
                context.PerformInvocation = false;
            }
        }
        public override void PostInterceptor(InterceptionContext context)
        {
            string cacheKey = string.Join(",", context.Arguments);
            object result = context.ReturnValue;

            //Set value in cache
            _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Cache set for {cacheKey}; value = {result}");
            _cache.Set(cacheKey, result);
        }
    }
}
