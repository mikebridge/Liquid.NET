using System;
using System.Linq;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    public class MissingVariableTests
    {
        [Theory]
        [InlineData("x", "x")]
        [InlineData("d.x", "x")]
        [InlineData("d[x]", "x")]
        [InlineData("a.b.c", "a")]
        public void It_Should_Display_An_Error_When_Dereferencing_Missing_Variable(String varname, String missingVar)
        {
            // Arrange

            ITemplateContext ctx = new TemplateContext()                              
                .ErrorWhenVariableMissing();
            ctx.DefineLocalVariable("d", new LiquidHash());
           
            // Act
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.Equal("Result : ERROR: " + missingVar + " is undefined", result.Result);

        }

        [Theory]
        [InlineData("e[1]")]
        [InlineData("e.first")]
        [InlineData("e.last")]
        public void It_Should_Display_An_Error_When_Dereferencing_Empty_Array(String varname)
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenVariableMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection());

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.Equal("Result : ERROR: cannot dereference empty array", result.Result);

        }



        [Fact]
        public void It_Should_Display_Error_When_Dereferencing_Array_With_Non_Int()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenVariableMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection());

            // Act
            var template = LiquidTemplate.Create("Result : {{ e.x }}");
            var result = template.LiquidTemplate.Render(ctx);
            //var result = RenderingHelper.RenderTemplate("Result : {{ e.x }}", ctx);

            // Assert
            Assert.Contains("invalid index: 'x'", result.Result);

        }

        [Fact]
        public void It_Should_Display_Error_When_Dereferencing_Primitive_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenVariableMissing();
            ctx.DefineLocalVariable("e", LiquidString.Create("Hello"));

            // Act
            var template = LiquidTemplate.Create("Result : {{ e.x }}");
            var result = template.LiquidTemplate.Render(ctx);

            Assert.True(result.HasRenderingErrors);
            var errorMessage = String.Join(",", result.RenderingErrors.Select(x => x.Message));
            // Assert
            Assert.Contains("invalid string index: 'x'", errorMessage);

        }


        [Theory]
        [InlineData("x")]
        [InlineData("e[1]")]
        [InlineData("e.x")]
        [InlineData("d.x")]
        [InlineData("d[x]")]
        [InlineData("a.b.c")]
        public void It_Should_Not_Display_An_Error_When_Dereferencing_Missing_Variable(String varname)
        {
            // Arrange
            Console.WriteLine(varname);
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection());
            ctx.DefineLocalVariable("d", new LiquidHash());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.Equal("Result : ", result);

        }



    }
}
