using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class TruncateFilterTests
    {
        [Theory]
        [InlineData("The cat came back the very next day", 10, "The cat...")]
        [InlineData("", 10, "")]
        [InlineData("abc", 0, "")]
        [InlineData("abc", 2, "ab")]
        [InlineData("abc", 3, "abc")]
        [InlineData("abc", 4, "abc")]
        [InlineData("abcd", 1, "a")]
        [InlineData("abcd", 2, "ab")]
        [InlineData("abcd", 3, "abc")]
        [InlineData("abcd", 4, "a...")]
        public void It_Should_Truncate_Strings(String original, int length, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '" + original + "' | truncate: " + length + " }}");
            // Act

            // Assert
            Assert.Equal("Result : " + expected, result);

        }

        [Fact]
        public void It_Should_Use_Something_Other_Than_Ellipses()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'I thought he was a goner' | truncate:12, '!!!' }}");
            // Act

            // Assert
            Assert.Equal("Result : I thought!!!", result);

        }

    }
}
