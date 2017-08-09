using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidStringTests
    {
        [Fact]
        public void It_Should_Store_The_Value()
        {
            // Arrange
            var stringSymbol = LiquidString.Create("String Test");
            var result = stringSymbol.Value;

            // Assert
            Assert.Equal("String Test", result);

        }

        [Fact]
        public void It_Should_Not_Allow_Null_Creation()
        {
            // Arrange
            var stringSymbol = LiquidString.Create(null);

            // Assert
            Assert.Null(stringSymbol);

        }

//        [Fact]
//        public void It_Should_Eval_A_Null_Value()
//        {
//            // Arrange
//            var stringSymbol = LiquidString.Create(null);
//            var result = stringSymbol.Eval(new TemplateContext(), new List<Option<ILiquidValue>>());
//
//            // Assert
//            Assert.Equal(stringSymbol, result.SuccessValue<LiquidString>());
//
//        }

        [Fact]
        public void It_Should_Join_Two_Values()
        {
            // Arrange
            var stringSymbol = LiquidString.Create("Hello");
            
            // Act
            LiquidString result = stringSymbol.Join(LiquidString.Create("World"));

            // Assert
            Assert.Equal("HelloWorld", result.StringVal);

        }

     

    }
}
