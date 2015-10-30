using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class UpCaseFilterTests
    {
        [Test]
        public void It_Should_Put_A_String_In_Upper_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST"));

        }

        [Test]
        public void It_Should_Not_Fail_For_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{  nil | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Not_Fail_For_Numerics()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 1 | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1"));

        }

        [Test]
        public void It_Should_Put_A_Bool_In_Upper_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ true | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TRUE"));

        }

        [Test]
        public void It_Should_Ignore_An_Extra_Arg()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Test\" | upcase: true }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST"));

        }

    }
}
