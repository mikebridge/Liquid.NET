using System;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        public void It_Should_Be_Right()
        {
            // Arrange
            var either = Either.Right<String, int>(123);
            // Act

            // Assert
            Assert.That(either.Right, Is.EqualTo(123));

        }

        [Test]
        public void It_Should_Be_Left()
        {
            // Arrange
            var either = Either.Left<String, int>("test");
            // Act

            // Assert
            Assert.That(either.Left, Is.EqualTo("test"));

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Not_Be_Left()
        {
            // Arrange
            var either = Either.Right<String, int>(123);
            // Act

            // Assert
            // ReSharper disable once UnusedVariable
            var result = either.Left;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Not_Be_Right()
        {
            // Arrange
            var either = Either.Left<String, int>("Test");
            // Act

            // Assert
            // ReSharper disable once UnusedVariable
            var result = either.Right;
        }

    }
}
