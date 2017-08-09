using System;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class EitherTests
    {
        [Fact]
        public void It_Should_Be_Right()
        {
            // Arrange
            var either = Either.Right<String, int>(123);
            // Act

            // Assert
            Assert.Equal(123, either.Right);

        }

        [Fact]
        public void It_Should_Be_Left()
        {
            // Arrange
            var either = Either.Left<String, int>("test");
            // Act

            // Assert
            Assert.Equal("test", either.Left);

        }

        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Not_Be_Left()
        {
            // Arrange
            var either = Either.Right<String, int>(123);
            // Act

            // Assert
            // ReSharper disable once UnusedVariable
            // var result = either.Left;
            Assert.Throws<InvalidOperationException>(() => either.Left);

            
        }

        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Not_Be_Right()
        {
            // Arrange
            var either = Either.Left<String, int>("Test");
            // Act

            // Assert
            // ReSharper disable once UnusedVariable
            //var result = either.Right;
            Assert.Throws<InvalidOperationException>(() => either.Right);
        }

    }
}
