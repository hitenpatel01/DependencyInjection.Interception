using Castle.DynamicProxy;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace DependencyInjection.Interception.Tests
{
    public class InterceptionContextTests
    {
        [Fact]
        public void Arguments_ShouldGetObjectArray()
        {
            //Arrange
            var expected = new object[] { };
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.Arguments == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.Arguments;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GenericArguments_ShouldGetTypeArray()
        {
            //Arrange
            var expected = new Type[] { };
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.GenericArguments == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.GenericArguments;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InvocationTarget_ShouldGetObject()
        {
            //Arrange
            var expected = new Object();
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.InvocationTarget == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.InvocationTarget;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Method_ShouldGetMethodInfo()
        {
            //Arrange
            var expected = MethodInfo.GetCurrentMethod() as MethodInfo;
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.Method == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.Method;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MethodInvocationTarget_ShouldGetMethodInfo()
        {
            //Arrange
            var expected = MethodInfo.GetCurrentMethod() as MethodInfo;
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.MethodInvocationTarget == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.MethodInvocationTarget;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Proxy_ShouldGetObject()
        {
            //Arrange
            var expected = new Object();
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.Proxy == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.Proxy;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnValue_ShouldGetObject()
        {
            //Arrange
            var expected = new Object();
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.ReturnValue == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.ReturnValue;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnValue_ShouldSetObject()
        {
            //Arrange
            var expected = new Object();
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>();
            var interceptionContext = new InterceptionContext(invocation);
            interceptionContext.ReturnValue = expected;

            //Act
            var actual = interceptionContext.ReturnValue;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TargetType_ShouldGetType()
        {
            //Arrange
            var expected = this.GetType();
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.TargetType == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.TargetType;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CaptureProceedInfo_ShouldReturnIInvocationProceedInfo()
        {
            //Arrange
            var expected = new object() as IInvocationProceedInfo;
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.CaptureProceedInfo() == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.CaptureProceedInfo();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetArgumentValue_ShouldReturnObject_ForEachIndex()
        {
            //Arrange
            var expected0 = new object();
            var expected1 = new object();
            var expected2 = new object();
            
            var invocationMock = new Mock<Castle.DynamicProxy.IInvocation>();
            
            invocationMock.Setup(x => x.GetArgumentValue(0)).Returns(expected0);
            invocationMock.Setup(x => x.GetArgumentValue(1)).Returns(expected1);
            invocationMock.Setup(x => x.GetArgumentValue(2)).Returns(expected2);

            var interceptionContext = new InterceptionContext(invocationMock.Object);

            //Act
            var actual0 = interceptionContext.GetArgumentValue(0);
            var actual1 = interceptionContext.GetArgumentValue(1);
            var actual2 = interceptionContext.GetArgumentValue(2);

            //Assert
            Assert.Equal(expected0, actual0);
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void GetConcreteMethod_ShouldGetConcreteMethod()
        {
            //Arrange
            var expected = MethodInfo.GetCurrentMethod() as MethodInfo;
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.GetConcreteMethod() == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.GetConcreteMethod();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetConcreteMethodInvocationTarget_ShouldGetConcreteMethod()
        {
            //Arrange
            var expected = MethodInfo.GetCurrentMethod() as MethodInfo;
            var invocation = Mock.Of<Castle.DynamicProxy.IInvocation>(x => x.GetConcreteMethod() == expected);
            var interceptionContext = new InterceptionContext(invocation);

            //Act
            var actual = interceptionContext.GetConcreteMethod();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetArgumentValue_ShouldSetObject_ForEachIndex()
        {
            //Arrange
            var expected0 = new object();
            var expected1 = new object();
            var expected2 = new object();

            var invocationMock = new Mock<Castle.DynamicProxy.IInvocation>();

            invocationMock.Setup(x => x.GetArgumentValue(0)).Returns(expected0);
            invocationMock.Setup(x => x.GetArgumentValue(1)).Returns(expected1);
            invocationMock.Setup(x => x.GetArgumentValue(2)).Returns(expected2);

            var interceptionContext = new InterceptionContext(invocationMock.Object);

            //Act
            var actual0 = interceptionContext.GetArgumentValue(0);
            var actual1 = interceptionContext.GetArgumentValue(1);
            var actual2 = interceptionContext.GetArgumentValue(2);

            //Assert
            Assert.Equal(expected0, actual0);
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void PerformInvocation_ShouldGetBoolean()
        {
            //Arrange
            var invocationMock = new Mock<Castle.DynamicProxy.IInvocation>();
            var interceptionContextMock = new Mock<InterceptionContext>(invocationMock.Object);
            var expected = true;
            interceptionContextMock.Setup(x => x.PerformInvocation).Returns(expected);

            //Act
            var actual = interceptionContextMock.Object.PerformInvocation;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PerformInvocation_ShouldSetBoolean()
        {
            //Arrange
            var invocationMock = new Mock<Castle.DynamicProxy.IInvocation>();
            var interceptionContext = new InterceptionContext(invocationMock.Object);
            
            var expected = true;
            interceptionContext.PerformInvocation = expected;

            //Act
            var actual = interceptionContext.PerformInvocation;
            
            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
