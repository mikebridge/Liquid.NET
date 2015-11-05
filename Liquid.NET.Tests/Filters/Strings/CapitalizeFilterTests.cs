using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class CapitalizeFilterTests
    {
        [Test]
        [TestCase("capitalize", "Capitalize")]
        [TestCase("", "")]
        [TestCase("a", "A")]
        [TestCase("the capital", "The capital")]
        [TestCase("  the capital  ", "  The capital  ")]
        [TestCase(null, "")]
        public void It_Should_Convert_Ruby_Case_To_Camel_Case(String before, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '" + before + "' | capitalize }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }
    }
}
