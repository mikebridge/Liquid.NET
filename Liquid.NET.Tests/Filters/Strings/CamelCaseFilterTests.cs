using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class CamelCaseFilterTests
    {
        [Theory]
        [InlineData("camel-case", "CamelCase")]
        [InlineData("", "")]
        [InlineData("a", "A")]
        [InlineData("camel case", "CamelCase")]
        [InlineData("camel    case", "CamelCase")]
        [InlineData("camel_case", "CamelCase")]
        public void It_Should_Convert_Ruby_Case_To_Camel_Case(String before, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ '"+before+"' | camelcase }}");

            // Assert
            Assert.Equal("Result : "+expected, result);                  

        }
    }
}

