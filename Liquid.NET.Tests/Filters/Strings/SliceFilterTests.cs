using System;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class SliceFilterTests
    {
        [Theory]
        [InlineData("Hello", "0", "H")]
        [InlineData("Hello", "1,3", "ell")]
        [InlineData("Hello", "1", "e")]
        [InlineData("Hello", "-3,2", "ll")] // Shopify example is incorrect
        [InlineData("Hello", "2,-3", "")]
        [InlineData("Hello", "3,1", "l")]
        [InlineData("Hello", "10,1", "")]
        public void It_Should_Slice_A_String(String orig, String slice, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"" + orig + "\" | slice : "+slice+" }}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

        [Theory]
        //[InlineData("0", "[ \"a string\" ]")]
        //[InlineData("1,3", "[ 123, 456, false ]")]
        //[InlineData("-3,2", "[ 123, 456 ]")]
        [InlineData("0", "a string")]
        [InlineData("1,3", "123456.0false")]
        [InlineData("-3,2", "123456.0")]
        
        public void It_Should_Slice_An_Array(String slice, string expected)
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArray());
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ array | slice : " + slice + " }}", ctx);

            // Assert
            Assert.Equal("Result : "+expected, result);


        }

        [Fact]
        public void It_Should_Not_Slice_A_Number()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("num", LiquidNumeric.Create(1));

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ num | slice : 1,2 }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ num | slice : 1,2 }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.Contains("Can't slice a object of type", result.Result);

        }

        [Fact]
        public void It_Should_Handle_Missing_Start_With_Arrays()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArray());
            
            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ array | slice }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ array | slice }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.Contains("Please pass a start parameter", result.Result);

        }

        [Fact]
        public void It_Should_Handle_Missing_Start_With_Strings()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("str", LiquidString.Create("Test"));
            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ str | slice }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ str | slice }}");
            var result = template.LiquidTemplate.Render(ctx);
            // Assert
            Assert.Contains("Please pass a start parameter", result.Result);

        }

        [Fact]
        public void It_Should_Slice_Nil()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            //ctx.DefineLocalVariable("array", CreateArray());
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ novar | slice }}", ctx);

            // Assert
            Assert.Contains("Result : ", result);

        }

        public LiquidCollection CreateArray()
        {
            return new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };

        }


        
    }
}
