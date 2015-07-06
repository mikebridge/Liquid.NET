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
        [TestCase("\"\"", "")]
        [TestCase("\"Test\"", "Test")]
        [TestCase("0", "0")]
        public void It_Should_Not_Return_Default_If_Not_Null(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ "+input+" | default: \"Hello\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));

        }

    }
}
