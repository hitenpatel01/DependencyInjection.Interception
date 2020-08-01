using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjection.Interception.Tests
{
    public class InterceptableAttributeTests
    {
        [Fact]
        void Interceptable_ShouldSetEnabledAsTrue_ByDefault()
        {
            var interceptableAttribute = new InterceptableAttribute();
            Assert.True(interceptableAttribute.Enabled);
        }

        [Fact]
        void Interceptable_ShouldSetEnabledAsFalse_WhenSet()
        {
            var interceptableAttribute = new InterceptableAttribute(false);
            Assert.False(interceptableAttribute.Enabled);
        }
    }
}
