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
    }
}
