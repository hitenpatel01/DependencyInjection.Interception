using DependencyInjection.Interception;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Castle.DynamicProxy;
using Moq;

namespace DependencyInjection.Interception.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Should_ReturnInterceptableServiceProvider()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            var provider = services.BuildServiceProviderWithInterception();

            //Assert
            Assert.IsType<InterceptableServiceProvider>(provider);
        }
    }
}
