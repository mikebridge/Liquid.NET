using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class RoundFilterTests
    {        
        [Theory]
        [InlineData("123.4", 123)]
        [InlineData("123.5", 124)]
        [InlineData("124.4", 124)]
        [InlineData("124.5", 125)]
        [InlineData("-1.3", -1)]
        [InlineData("-1.5", -2)]
        public void It_Should_Round_To_Nearest_Whole_Number_Away_From_Zero(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ "+input+" | round }}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

        [Theory]
        [InlineData("123.33333333333", 2, "123.33")]
        [InlineData("123.5", 0, "124")]
        [InlineData("124.45", 1, "124.5")]
        [InlineData("124.499", 1, "124.5")]
        [InlineData("-1.3", -1, "-1")]
        [InlineData("-1.5", 0, "-2")]
        //[InlineData("1", null, "1")]
        public void It_Should_Round_To_Nearest_Decimal_Number(String input, int digits, String expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | round : "+digits+" }}");

            // Assert
            Assert.Equal("Result : " +expected, result);
        }

        [Fact]
        public void It_Should_Round_To_Int_When_No_Arg()
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ 12.5 | round }}");

            // Assert
            Assert.Equal("Result : 13", result);
        }


    }
}
