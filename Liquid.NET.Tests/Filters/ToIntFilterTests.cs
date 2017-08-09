using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class ToIntFilterTests
    {
        [Theory]
        [InlineData("123", 123)]
        [InlineData("123.45", 123)]
        [InlineData("123.5", 124)]
        [InlineData("124.5", 125)]
        [InlineData("-124.5", -125)]
        [InlineData("-123.5", -124)]
        [InlineData("wefwef", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void It_Should_Cast_A_String_To_An_Int(String input, int expected)
        {
            // Arrange
            var toIntFilter = new ToIntFilter();

            // Act
            var result = toIntFilter.Apply(new TemplateContext(), LiquidString.Create(input)).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.Equal((decimal) expected, result.DecimalValue);
            Assert.Equal(expected, result.IntValue);
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        public void It_Should_Cast_A_Bool_To_An_Int(bool val, int expected)
        {
            // Arrange
            var toIntFilter = new ToIntFilter();

            // Act
            var result = toIntFilter.Apply(new TemplateContext(), new LiquidBoolean(val)).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.Equal((decimal)expected, result.DecimalValue);
            Assert.Equal(expected, result.IntValue);

        }

       

    }
}
