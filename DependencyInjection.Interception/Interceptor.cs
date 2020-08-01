using Castle.DynamicProxy;

namespace DependencyInjection.Interception
{
    public abstract class Interceptor : IInterceptor
    {
        internal Interceptor Next { get; set; }
        public virtual void PreInterceptor(InterceptionContext context) { }
        public virtual void PostInterceptor(InterceptionContext context) { }
        public virtual void Intercept(IInvocation invocation)
        {
            var context = new InterceptionContext(invocation);
            PreInterceptor(context);
            if (null != Next)
            {
                Next.Intercept(invocation);
            }
            else
            {
                if (context.PerformInvocation)
                {
                    invocation.Proceed();
                }
            }
            PostInterceptor(context);
        }
    }
}
