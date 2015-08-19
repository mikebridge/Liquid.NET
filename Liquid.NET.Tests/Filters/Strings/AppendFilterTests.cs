using System;

using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class AppendFilterTests
    {
        [Test]
        public void It_Should_Append_Text_To_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | append : \".jpg\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : test.jpg"));
        }

        [Test]
        public void It_Should_Append_Text_To_Empty()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"\" | append : \"jpg\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : jpg"));
        }

        [Test]
        public void It_Should_Append_Text_To_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123 | append : \".jpg\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123.jpg"));
        }

        [Test]
        public void It_Should_Append_Text_To_Nothing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ x | append : \".jpg\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : .jpg"));
        }

        [Test] public void It_Should_Append_Nil_To_Nothing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'Test' | append : x }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Test"));
        }
    }
}
