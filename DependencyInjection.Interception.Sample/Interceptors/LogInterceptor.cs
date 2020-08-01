using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Interception.Sample.Interceptors
{
    public class LogInterceptor : Interceptor
    {
        private readonly ILogger<LogInterceptor> _logger;
        public LogInterceptor(ILogger<LogInterceptor> logger) => _logger = logger;
        public override void PreInterceptor(InterceptionContext context)
        {
            _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Executing with arguments {string.Join(',', context.Arguments)}");
        }

        public override void PostInterceptor(InterceptionContext context)
        {
            _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Executed. Return value {context.ReturnValue}");
        }
    }
}
