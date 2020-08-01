using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjection.Interception.Tests
{
    public partial class IntegrationTests
    {
        public class TestInterceptor : Interceptor { }
        public class TestInterceptor2 : Interceptor { }
        public class TestInterceptor3 : Interceptor { }
        public class TestInterceptor4 : Interceptor { }
        public class InvalidTestInterceptor { }

        [Interceptable]
        [Interceptor(typeof(TestInterceptor))]
        [Interceptor(typeof(TestInterceptor2))]
        public class TestServiceWithMultipleInterceptors
        {
            [Interceptor(typeof(TestInterceptor3))]
            [Interceptor(typeof(TestInterceptor4))]
            public virtual void TestMethod() { }
        }

        ServiceCollection services = null;

        Mock<TestInterceptor> mockInterceptor = null;
        Mock<TestInterceptor2> mockInterceptor2 = null;
        Mock<TestInterceptor3> mockInterceptor3 = null;
        Mock<TestInterceptor4> mockInterceptor4 = null;

        Mock<TestServiceWithMultipleInterceptors> mockServiceWithMultipleInterceptors = null;

        public IntegrationTests()
        {
            services = new ServiceCollection();

            mockInterceptor = new Mock<TestInterceptor>();
            mockInterceptor.CallBase = true;

            mockInterceptor2 = new Mock<TestInterceptor2>();
            mockInterceptor2.CallBase = true;

            mockInterceptor3 = new Mock<TestInterceptor3>();
            mockInterceptor3.CallBase = true;

            mockInterceptor4 = new Mock<TestInterceptor4>();
            mockInterceptor4.CallBase = true;

            mockServiceWithMultipleInterceptors = new Mock<TestServiceWithMultipleInterceptors>();
            mockServiceWithMultipleInterceptors.CallBase = true;
        }

        [Fact]
        public void Should_InterceptMethod_WhenTransientService()
        {
            //Arrange
            ConfigureInterceptors();

            services
                .AddTransient<TestServiceWithMultipleInterceptors>(provider => mockServiceWithMultipleInterceptors.Object)
                .BuildServiceProviderWithInterception()
                .GetService<TestServiceWithMultipleInterceptors>()

            //Act
                .TestMethod();

            //Assert
            Verify();
        }

        [Fact]
        public void Should_InterceptMethod_WhenSingletonService()
        {
            //Arrange
            ConfigureInterceptors();

            services
                .AddSingleton<TestServiceWithMultipleInterceptors>(provider => mockServiceWithMultipleInterceptors.Object)
                .BuildServiceProviderWithInterception()
                .GetService<TestServiceWithMultipleInterceptors>()

            //Act
                .TestMethod();

            //Assert
            Verify();
        }

        [Fact]
        public void Should_InterceptMethod_WhenScopedService()
        {
            //Arrange
            ConfigureInterceptors();

            var interceptableServiceProvider = services
                .AddScoped<TestServiceWithMultipleInterceptors>(provider => mockServiceWithMultipleInterceptors.Object)
                .BuildServiceProviderWithInterception() as IInterceptableServiceProvider;

            interceptableServiceProvider
                .CreateInterceptableScope()
                .ServiceProvider
                .GetService<TestServiceWithMultipleInterceptors>()

            //Act
                .TestMethod();

            //Assert
            Verify();
        }

        private void ConfigureInterceptors()
        {
            services.AddTransient<TestInterceptor>(provider => mockInterceptor.Object)
                .AddTransient<TestInterceptor2>(provider => mockInterceptor2.Object)
                .AddTransient<TestInterceptor3>(provider => mockInterceptor3.Object)
                .AddTransient<TestInterceptor4>(provider => mockInterceptor4.Object);
        }

        private void Verify()
        {
            mockInterceptor.Verify(interceptor => interceptor.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor2.Verify(interceptor => interceptor.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor3.Verify(interceptor => interceptor.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor4.Verify(interceptor => interceptor.PreInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockServiceWithMultipleInterceptors.Verify(interceptor => interceptor.TestMethod(), Times.Once());
            mockInterceptor.Verify(interceptor => interceptor.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor2.Verify(interceptor => interceptor.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor3.Verify(interceptor => interceptor.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
            mockInterceptor4.Verify(interceptor => interceptor.PostInterceptor(It.IsAny<InterceptionContext>()), Times.Once());
        }
    }
}
