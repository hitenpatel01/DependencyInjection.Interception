using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjection.Interception.Tests
{
    public class InterceptorAttributeTests
    {
        class ValidInterceptor : Interceptor { }
        class InValidInterceptor { }

        [Fact]
        public void Should_AssignInterceptorType_WhenTypeIsSubClassOfInterceptor()
        {
            var type = typeof(ValidInterceptor);
            var interceptorAttribute = new InterceptorAttribute(type);
            Assert.Equal(type, interceptorAttribute.InterceptorType);
        }

        [Fact]
        public void Should_ThrowArgumentException_WhenTypeIsNotSubClassOfInterceptor()
        {
            var exception = Assert.ThrowsAny<ArgumentException>(() => new InterceptorAttribute(typeof(InValidInterceptor)));
            Assert.Equal($"interceptorType should be of type {nameof(Interceptor)}", exception.Message);
        }
    }
}
