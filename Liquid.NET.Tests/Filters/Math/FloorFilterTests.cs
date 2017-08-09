using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class FloorFilterTests
    {
        [Theory]
        [InlineData("123.4", 123)]
        [InlineData("123", 123)]
        [InlineData("124.4", 124)]
        [InlineData("-1.3", -2)]
        [InlineData("-1.5", -2)]
        public void It_Should_Find_The_Floor_Value(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | floor }}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

    }
}
