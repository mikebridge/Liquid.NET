using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class TruncateFilterTests
    {
        [Test]
        [TestCase("The cat came back the very next day", 10, "The cat...")]
        [TestCase("", 10, "")]
        [TestCase("abc", 0, "")]
        [TestCase("abc", 2, "ab")]
        [TestCase("abc", 3, "abc")]
        [TestCase("abc", 4, "abc")]
        [TestCase("abcd", 1, "a")]
        [TestCase("abcd", 2, "ab")]
        [TestCase("abcd", 3, "abc")]
        [TestCase("abcd", 4, "a...")]
        public void It_Should_Truncate_Strings(String original, int length, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '" + original + "' | truncate: " + length + " }}");
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Use_Something_Other_Than_Ellipses()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'I thought he was a goner' | truncate:12, '!!!' }}");
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : I thought!!!"));

        }

    }
}
