using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace DependencyInjection.Interception
{
    public class InterceptionContext
    {
        private bool _performInvocation = true;
        public object[] Arguments { get => invocation.Arguments; }
        public Type[] GenericArguments { get => invocation.GenericArguments; }
        public object InvocationTarget { get => invocation.InvocationTarget; }
        public MethodInfo Method { get => invocation.Method; }
        public MethodInfo MethodInvocationTarget { get => invocation.MethodInvocationTarget; }
        public object Proxy { get => invocation.Proxy; }
        public object ReturnValue { get => invocation.ReturnValue; set => invocation.ReturnValue = value; }
        public Type TargetType { get => invocation.TargetType; }

        protected readonly IInvocation invocation;
        
        public InterceptionContext(IInvocation invocation) => this.invocation = invocation;

        public IInvocationProceedInfo CaptureProceedInfo() => invocation.CaptureProceedInfo();
        public object GetArgumentValue(int index) => invocation.GetArgumentValue(index);
        public MethodInfo GetConcreteMethod() => invocation.GetConcreteMethod();
        public MethodInfo GetConcreteMethodInvocationTarget() => invocation.GetConcreteMethodInvocationTarget();
        public void SetArgumentValue(int index, object value) => invocation.SetArgumentValue(index, value);
        public virtual bool PerformInvocation { get => _performInvocation; set => _performInvocation = value; }
    }
}
