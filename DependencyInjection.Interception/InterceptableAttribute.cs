using System;

namespace DependencyInjection.Interception
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class InterceptableAttribute : Attribute
    {
        public bool Enabled{ get; protected set; }
        public InterceptableAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}
