using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using DependencyInjection.Interception;

namespace DependencyInjection.Interception.Sample
{
    public class AuthorizationInterceptor : Interceptor
    {
        private readonly ILogger<AuthorizationInterceptor> _logger;
        public AuthorizationInterceptor(ILogger<AuthorizationInterceptor> logger)
        {
            _logger = logger;
        }
        public override void PreInterceptor(InterceptionContext context) 
        {
            _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Authorizing user '{Thread.CurrentPrincipal.Identity.Name}'");
            if (false == Thread.CurrentPrincipal.IsInRole("AuthorizedUser"))
            {
                _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Authorization failed for user '{Thread.CurrentPrincipal.Identity.Name}'");
                throw new UnauthorizedAccessException($"{Thread.CurrentPrincipal.Identity.Name} is not authorized to call {context.TargetType.Name}::{context.Method.Name}");
            }
            else
            {
                _logger.LogInformation($"{context.TargetType.Name}::{context.Method.Name} - Authorization successful for user '{Thread.CurrentPrincipal.Identity.Name}'");
            }
        }

        //Nothing to do in post interception, hence no need to override this method
        //public override void PostInterceptor(InterceptionContext context) 
        //{   
        //}
    }
}
