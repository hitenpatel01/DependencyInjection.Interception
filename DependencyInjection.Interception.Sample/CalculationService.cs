using Castle.Core.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DependencyInjection.Interception.Sample
{
    public class CalculationService : ICalculationService
    {
        private readonly ILogger<CalculationService> _logger;
        private readonly IMemoryCache _cache;
        public CalculationService(ILogger<CalculationService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public int Sum(int a, int b)
        {
            var caller = $"{nameof(CalculationService)}::{nameof(Sum)}";

            //Log method execution
            _logger.LogTrace($"{caller} - Executing with arguments {String.Join(',', new[] {a, b})}");

            //Perform authorization check
            _logger.LogTrace($"{caller} - Authorizing user '{Thread.CurrentPrincipal.Identity.Name}'");
            if (false == Thread.CurrentPrincipal.IsInRole("AuthorizedUser"))
            {
                _logger.LogError($"{caller} - Authorization failed for user '{Thread.CurrentPrincipal.Identity.Name}'");
                throw new UnauthorizedAccessException($"{Thread.CurrentPrincipal.Identity.Name} is not authorized to call {caller}");
            }
            else 
            {
                _logger.LogInformation($"{caller} - Authorization successful for user '{Thread.CurrentPrincipal.Identity.Name}'");
            }

            int result;

            //Check cached value
            var cacheKey = $"{a},{b}";
            if (false == _cache.TryGetValue<int>(cacheKey, out result))
            {
                _logger.LogWarning($"{caller} - Cache miss for {cacheKey}");
                
                //Compute Result
                result = a + b;

                //Set value in cache
                _logger.LogInformation($"{caller} - Cache set for {cacheKey}; value = {result}");
                _cache.Set<double>(cacheKey, result);
            }
            else
            {
                _logger.LogTrace($"{caller} - Cache hit for {cacheKey}");
            }

            //Log result
            _logger.LogTrace($"{caller} - Executed. Return value {result}");

            return result;
        }
    }
}
