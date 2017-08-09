using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class CeilFilterTests
    {
        [Theory]
        [InlineData("123.4", 124)]
        [InlineData("123", 123)]
        [InlineData("124.4", 125)]
        [InlineData("-1.3", -1)]
        [InlineData("-1.5", -1)]
        public void It_Should_Find_The_Ceil_Value(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | ceil }}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

    }
}
