using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjection.Interception.Tests
{
    public class InterceptableServiceProviderTests
    {
        public interface ITestService{ }

        public class TestService : ITestService
        {
            public void TestMethod() { }
        }

        [Fact]
        public void Should_ReturnInterceptableService_WhenServiceTypeIsInterface()
        {
            var provider = new ServiceCollection()
                .AddTransient<ITestService, TestService>()
                .BuildServiceProviderWithInterception() as InterceptableServiceProvider;
            var service = provider.GetService(typeof(ITestService));
            Assert.IsAssignableFrom<ITestService>(service);
        }

        [Fact]
        public void Should_ReturnInterceptableService_WhenServiceTypeIsClass()
        {
            var provider = new ServiceCollection()
                .AddTransient<TestService>()
                .BuildServiceProviderWithInterception() as InterceptableServiceProvider;
            var service = provider.GetService(typeof(TestService));
            Assert.IsAssignableFrom<TestService>(service);
        }

        [Fact]
        public void Should_ThrowsArgumentNullException_WhenServiceProviderArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new InterceptableServiceProvider(null); });
        }

        [Fact]
        public void Should_ThrowsArgumentNullException_WhenServiceTypeArgumentIsNull()
        {
            var provider = new ServiceCollection().BuildServiceProvider();
            Assert.Throws<ArgumentNullException>(() => { new InterceptableServiceProvider(provider).GetService(null); });
        }

        [Fact]
        public void Should_ThrowsNullReferenceException_WhenProxyGeneratorIsNotConfigured()
        {
            var provider = new ServiceCollection()
                .AddTransient<ITestService, TestService>()
                .BuildServiceProvider();
            Assert.Throws<NullReferenceException>(() => { new InterceptableServiceProvider(provider).GetService(typeof(ITestService)); });
        }
    }
}
