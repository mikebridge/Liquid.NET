using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Filters.Array
{
    
    public class JoinFilterTests
    {
        [Fact]
        public void It_Should_Join_An_Array_With_A_String()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new JoinFilter(LiquidString.Create(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidString>();
            //Logger.Log(result.ToString());

            // Assert
            Assert.Equal("a string, 123, 456.0, false", result.StringVal);
     
        }

        [Fact]
        public void It_Should_Join_String_With_A_String()
        {
            // Arrange
                
            var filter = new JoinFilter(LiquidString.Create(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create("Hello World")).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("H, e, l, l, o,  , W, o, r, l, d", result.Value);

        }

        [Fact]        
        public void It_Should_Join_With_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("{% assign arr = 'a,b,c' | split: ',' %}Result : {{ arr| join }}");

            // Assert
            Assert.Equal("Result : abc", result);
        }

    }
}
