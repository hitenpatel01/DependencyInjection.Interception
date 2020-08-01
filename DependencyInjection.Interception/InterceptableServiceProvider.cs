using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DependencyInjection.Interception
{
    public interface IInterceptableServiceProvider : IServiceProvider, IDisposable
    {
        IServiceScope CreateInterceptableScope();
    }

    public class InterceptableServiceProvider : IInterceptableServiceProvider, IServiceProvider, IDisposable
    {
        protected bool _disposed = false;
        protected ServiceProvider _provider;
        public InterceptableServiceProvider(ServiceProvider provider)
        {
            if (null == provider) throw new ArgumentNullException($"{nameof(provider)}");
            _provider = provider;
        }
        ~InterceptableServiceProvider()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_disposed) return;
            if (isDisposing) _provider.Dispose();
            _disposed = true;
        }

        public object GetService(Type serviceType)
        {
            if (null == serviceType) throw new ArgumentNullException($"{nameof(serviceType)}");

            var proxyGenerator = _provider.GetService<ProxyGenerator>();
            if (null == proxyGenerator) throw new NullReferenceException($"{nameof(ProxyGenerator)} must be added to service provider");

            var target = _provider.GetService(serviceType);
            if (null == target) throw new NullReferenceException($"Null returned by {nameof(ServiceProvider)} for {serviceType.FullName}");

            var interceptableAttribute = Attribute.GetCustomAttribute(target.GetType(), typeof(InterceptableAttribute)) as InterceptableAttribute;

            if (null == interceptableAttribute || false == interceptableAttribute.Enabled) return target;

            var interceptors = Attribute.GetCustomAttributes(target.GetType(), typeof(InterceptorAttribute))
                .Cast<InterceptorAttribute>()
                .Select(x => _provider.GetService(x.InterceptorType) as Interceptor)
                .Union(new[] { _provider.GetService<MethodInterceptionHandler>() })
                .ToList();

            for (var index = 0; index < interceptors.Count() - 1; index++)
            {
                interceptors[index].Next = interceptors[index + 1];
            }

            IInterceptor primary = interceptors.FirstOrDefault();

            return serviceType.IsInterface
                    ? _provider
                        .GetService<ProxyGenerator>()
                        .CreateInterfaceProxyWithTarget(serviceType, target, primary)
                    : _provider
                        .GetService<ProxyGenerator>()
                        .CreateClassProxyWithTarget(serviceType, target, primary);
        }
        public IServiceScope CreateInterceptableScope()
        {
            return new InterceptableServiceScope(_provider);
        }
    }

    public class InterceptableServiceScope : IServiceScope
    {
        protected bool _disposed = false;
        protected ServiceProvider _provider;
        protected InterceptableServiceProvider _interceptableServiceProvider;
        public InterceptableServiceScope(ServiceProvider provider)
        {
            if (null == provider) throw new ArgumentNullException($"{nameof(provider)}");
            _provider = provider;
        }
        ~InterceptableServiceScope()
        {
            Dispose(false);
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                _interceptableServiceProvider = _interceptableServiceProvider ?? new InterceptableServiceProvider(_provider);
                return _interceptableServiceProvider;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
           if (_disposed) return;
            if (isDisposing)
            {
                _interceptableServiceProvider.Dispose();
                _provider.Dispose();
            }

            _disposed = true;
        }
    }

}
