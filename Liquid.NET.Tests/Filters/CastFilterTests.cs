using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class CastFilterTests
    {
        [Fact]
        public void It_Should_Cast_The_Value_From_One_Type_To_Another()
        {
            // Arrange
            var castFilter = new CastFilter<LiquidString, LiquidNumeric>();
            var inputObj = LiquidString.Create("123");

            // Act
            var result = castFilter.Apply(new TemplateContext(),inputObj).SuccessValue<LiquidNumeric>();
            //result.Eval(new SymbolTableStack(new TemplateContext()), new List<ILiquidValue>());

            // Assert
            Assert.IsAssignableFrom<LiquidNumeric>(result);
            Assert.Equal(123m, result.DecimalValue);

        }

        [Theory]
        [InlineData(123.0, "123.0")]
        [InlineData(123, "123.0")]
        [InlineData(123.1, "123.1")]
        public void It_Should_Cast_A_Decimal_To_A_String_Like_Ruby_Liquid(decimal input, String expected)
        {
            // Arrange
            var castFilter = new CastFilter<LiquidNumeric, LiquidString>();

            // Act
            var result = castFilter.Apply(new TemplateContext(), LiquidNumeric.Create(input)).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal(expected, result.StringVal);


        }

        [Theory]
        [InlineData(123, "123")]
        public void It_Should_Cast_An_Int_To_A_String_Like_Ruby_Liquid(int input, String expected)
        {
            // Arrange
            var castFilter = new CastFilter<LiquidNumeric, LiquidString>();

            // Act
            var result = castFilter.Apply(new TemplateContext(), LiquidNumeric.Create(input)).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal(expected, result.StringVal);


        }

    }
}
