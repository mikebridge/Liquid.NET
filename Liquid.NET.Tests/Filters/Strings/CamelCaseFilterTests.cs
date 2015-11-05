using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class CamelCaseFilterTests
    {
        [Test]
        [TestCase("camel-case", "CamelCase")]
        [TestCase("", "")]
        [TestCase("a", "A")]
        [TestCase("camel case", "CamelCase")]
        [TestCase("camel    case", "CamelCase")]
        [TestCase("camel_case", "CamelCase")]
        public void It_Should_Convert_Ruby_Case_To_Camel_Case(String before, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '"+before+"' | camelcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));                  

        }
    }
}

