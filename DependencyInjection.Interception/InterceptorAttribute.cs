using System;
using System.Reflection;

namespace DependencyInjection.Interception
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InterceptorAttribute : Attribute
    {
        public Type InterceptorType { get; protected set; }
        public InterceptorAttribute(Type interceptorType)
        {
            if (!((TypeInfo)interceptorType).IsSubclassOf(typeof(Interceptor)))
            {
                throw new ArgumentException($"interceptorType should be of type {nameof(Interceptor)}");
            }
            InterceptorType = interceptorType;
        }
    }
}
