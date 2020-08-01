using Castle.DynamicProxy;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjection.Interception.Tests
{
    public class InterceptorTests
    {
        public class TestInterceptor : Interceptor
        {
            public override void PreInterceptor(InterceptionContext context)
            {
                context.PerformInvocation = false;
                base.PreInterceptor(context);
            }
            public override void PostInterceptor(InterceptionContext context)
            {
                base.PostInterceptor(context);
            }
        }

        [Fact]
        public void Interceptor_ShouldInvokePrePostAndInterceptorMethods_WhenPerformInterceptionIsNotReset()
        {
            //Arrange
            var mockInterceptor = new Mock<Interceptor> { CallBase = true };
            var mockInvocation = new Mock<Castle.DynamicProxy.IInvocation>();

            //Act
            mockInterceptor.Object.Intercept(mockInvocation.Object);

            //Assert
            mockInterceptor.Verify(m => m.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor.Verify(m => m.Intercept(It.IsAny<Castle.DynamicProxy.IInvocation>()), Times.Once());
            mockInterceptor.Verify(m => m.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInvocation.Verify(m => m.Proceed(), Times.Once());
        }

        [Fact]
        public void Interceptor_ShouldInvokePreAndPostInterceptorMethodsOnly_WhenPerformInterceptionIsReset()
        {
            //Arrange
            var mockInterceptor = new Mock<TestInterceptor> { CallBase = true };
            var mockInvocation = new Mock<Castle.DynamicProxy.IInvocation>();
            
            //Act
            mockInterceptor.Object.Intercept(mockInvocation.Object);

            //Assert
            mockInterceptor.Verify(m => m.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor.Verify(m => m.Intercept(It.IsAny<Castle.DynamicProxy.IInvocation>()), Times.Once());
            mockInterceptor.Verify(m => m.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInvocation.Verify(m => m.Proceed(), Times.Never());

        }
    }
}
