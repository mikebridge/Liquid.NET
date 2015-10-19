using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class DefaultFilterTests
    {
        [Test]
        public void It_Should_Return_Default_If_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | default: \"Hello\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello"));

        }

        [Test]
        public void It_Should_Return_Default_If_Empty_String()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"\" | default: \"Hello\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello"));

        }

        [Test]
        [TestCase("\"  \"", "  ")]
        [TestCase("\"Test\"", "Test")]
        [TestCase("0", "0")]
        public void It_Should_Not_Return_Default_If_Not_Null_And_Not_Empty(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ "+input+" | default: \"Hello\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));

        }

        [Test]
        public void It_Should_Return_Default_If_Array_Has_No_Elements()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign arr=\"\" | split: \"|\"%}{{ arr | default: \"DEFAULT\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : DEFAULT"));
        }

        [Test]
        public void It_Should_Return_Default_If_Array_Is_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ arr | default: \"DEFAULT\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : DEFAULT"));
        }

    }
}
