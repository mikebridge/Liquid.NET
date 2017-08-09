using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class ModuloFilterTests
    {
        [Theory]
        [InlineData("5", "3", "2")]
        [InlineData("2", "3", "2")]
        [InlineData("0", "2", "0")]
        public void It_Should_Modulo_Two_Numbers(String arg1, String arg2, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + arg1 + "  | modulo: " + arg2 + " }}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

        [Fact]
        public void It_Should_CHeck_For_Div_By_Zero()
        {
            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ 2  | modulo: 0 }}");
            var template = LiquidTemplate.Create("Result : {{ 2  | modulo: 0 }}");
            var result = template.LiquidTemplate.Render(new TemplateContext().WithAllFilters());
            // Assert
            Assert.Contains("Liquid error: divided by 0", result.Result);
        }


    }
}
