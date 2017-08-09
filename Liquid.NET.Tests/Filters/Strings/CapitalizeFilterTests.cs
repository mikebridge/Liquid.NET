using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class CapitalizeFilterTests
    {
        [Theory]
        [InlineData("capitalize", "Capitalize")]
        [InlineData("", "")]
        [InlineData("a", "A")]
        [InlineData("the capital", "The capital")]
        [InlineData("  the capital  ", "  The capital  ")]
        [InlineData(null, "")]
        public void It_Should_Convert_Ruby_Case_To_Camel_Case(String before, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '" + before + "' | capitalize }}");

            // Assert
            Assert.Equal("Result : " + expected, result);

        }
    }
}
