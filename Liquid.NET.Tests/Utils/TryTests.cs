using System;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class TryTests
    {
        [Fact]
        public void It_Should_Show_Success()
        {
            // Arrange
            var result = new Success<String>("Test");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void It_Should_Show_Failure()
        {
            // Arrange
            var result = new Failure<String>(new Exception("TEst"));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
        }

        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void Failure_Does_Not_Have_A_Value()
        {
            // Arrange
            var result = new Failure<String>(new Exception("TEst"));

            // Assert
            // ReSharper disable once UnusedVariable
            //var val = result.Value;
            Assert.Throws<InvalidOperationException>(() => result.Value);

        }

        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void Success_Does_Not_Have_An_Exception()
        {
            // Arrange
            var result = new Success<String>("Test");

            // Assert
            // ReSharper disable once UnusedVariable
            //var val = result.Exception;
            Assert.Throws<InvalidOperationException>(() => result.Exception);
        }

        [Fact]
        public void Success_Has_A_Value()
        {
            // Arrange
            var result = new Success<String>("TEst");

            // Assert
            Assert.Equal("TEst", result.Value);
        }

        [Fact]
        public void Failure_Has_An_Exception()
        {
            // Arrange
            var result = new Failure<String>(new Exception("Test"));

            // Assert
            Assert.Equal("Test", result.Exception.Message);
        }

    }
}
