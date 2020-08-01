using Castle.DynamicProxy;
using System;
using System.Linq;

namespace DependencyInjection.Interception
{
    public class MethodInterceptionHandler : Interceptor
    {
        private readonly IServiceProvider _provider;
        public MethodInterceptionHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override void Intercept(IInvocation invocation)
        {
            /* Get all InterceptorAttribute applied to method being intercepted 
             * and instantiate interceptors using IServiceProvider */
            var methodInterceptors = Attribute.GetCustomAttributes(invocation.MethodInvocationTarget, typeof(InterceptorAttribute))
                .Cast<InterceptorAttribute>()
                .Select(x => _provider.GetService(x.InterceptorType) as Interceptor)
                
                /* Assigning interceptors to .Next property creates a chain of 
                 * intercepts. If multiple singleton interceptors of same type 
                 * are associated to service then it will cause infinite chain
                 * which is broken by Distint() */
                .Distinct()
                .ToArray();

            if (methodInterceptors.Any())
            {
                for (var index = 0; index < methodInterceptors.Count() - 1; index++)
                {
                    methodInterceptors[index].Next = methodInterceptors[index + 1];
                }
                methodInterceptors.FirstOrDefault().Intercept(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
