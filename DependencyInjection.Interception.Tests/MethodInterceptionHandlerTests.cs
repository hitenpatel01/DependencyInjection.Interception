using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using IInvocation = Castle.DynamicProxy.IInvocation;

namespace DependencyInjection.Interception.Tests
{
    public class MethodInterceptionHandlerTests
    {
        public class TestInterceptor : Interceptor { }

        public interface ITestService
        {
            public void TestMethodWithoutInterceptor();
            public void TestMethodWithInterceptor();
        }

        public class TestService : ITestService
        {
            public void TestMethodWithoutInterceptor() { }

            [Interceptor(typeof(TestInterceptor))]
            public void TestMethodWithInterceptor() { }
        }

        [Fact]
        public void Should_InvokeMethodWithoutInterceptor()
        {
            //Arrange
            var provider = new ServiceCollection().BuildServiceProviderWithInterception();

            var methodInterceptionHandler = new MethodInterceptionHandler(provider);

            var methodInfoForTestMethodWithoutInterceptor = typeof(TestService).GetMethod("TestMethodWithoutInterceptor");
            var invocation = new Mock<IInvocation>();
            invocation.Setup(x => x.MethodInvocationTarget).Returns(methodInfoForTestMethodWithoutInterceptor);

            //Act
            methodInterceptionHandler.Intercept(invocation.Object);

            //Assert
            invocation.Verify(x => x.Proceed(), Times.Once);
        }

        [Fact]
        public void Should_InvokeMethodWithInterceptor()
        {
            //Arrange
            var interceptorMock = new Mock<TestInterceptor>();
            interceptorMock.CallBase = true;

            var provider = new ServiceCollection()
                .AddTransient<TestInterceptor>(provider => interceptorMock.Object)
                .BuildServiceProviderWithInterception();

            var methodInterceptionHandler = new MethodInterceptionHandler(provider);

            var methodInfoForTestMethodWithInterceptor = typeof(TestService).GetMethod("TestMethodWithInterceptor");

            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(x => x.MethodInvocationTarget).Returns(methodInfoForTestMethodWithInterceptor);

            //Act
            methodInterceptionHandler.Intercept(invocationMock.Object);

            //Assert
            interceptorMock.Verify(x => x.Intercept(It.IsAny<IInvocation>()), Times.Once);
            invocationMock.Verify(x => x.Proceed(), Times.Once);
        }
    }
}
