using System;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class TryTests
    {
        [Test]
        public void It_Should_Show_Success()
        {
            // Arrange
            var result = new Success<String>("Test");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.IsFailure, Is.False);
        }

        [Test]
        public void It_Should_Show_Failure()
        {
            // Arrange
            var result = new Failure<String>(new Exception("TEst"));

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.IsFailure, Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Failure_Does_Not_Have_A_Value()
        {
            // Arrange
            var result = new Failure<String>(new Exception("TEst"));

            // Assert
            // ReSharper disable once UnusedVariable
            var val = result.Value;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Success_Does_Not_Have_An_Exception()
        {
            // Arrange
            var result = new Success<String>("Test");

            // Assert
            // ReSharper disable once UnusedVariable
            var val = result.Exception;
        }

        [Test]
        public void Success_Has_A_Value()
        {
            // Arrange
            var result = new Success<String>("TEst");

            // Assert
            Assert.That(result.Value, Is.EqualTo("TEst"));
        }

        [Test]
        public void Failure_Has_An_Exception()
        {
            // Arrange
            var result = new Failure<String>(new Exception("Test"));

            // Assert
            Assert.That(result.Exception.Message, Is.EqualTo("Test"));
        }

    }
}
