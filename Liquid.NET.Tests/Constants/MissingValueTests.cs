using System;
using System.Linq;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    public class MissingValueTests
    {
        [Theory]
        [InlineData("x", "x")]
        [InlineData("d.x", "x")]
        [InlineData("d[x]", "x")]
        public void It_Should_Display_An_Error_When_Value_Is_Undefined(String varname, String missingVar)
        {
            // Arrange

            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("x", LiquidValue.None);
            ctx.DefineLocalVariable("d", new LiquidHash()
            {
                {
                    "x", LiquidValue.None
                }
            });

            // Act
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);
            //Console.WriteLine(result);

            // Assert
            Assert.Equal("Result : ERROR: " + missingVar + " is undefined", result.Result);
        }

        [Theory]
        [InlineData("e[1]", "1")]
        [InlineData("e.first", "first")]
        [InlineData("e.last", "last")]
        public void It_Should_Display_An_Error_When_Array_Value_Is_Undefined(String varname, String missingVar)
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection(new[] { LiquidValue.None, LiquidValue.None, LiquidValue.None }));

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.Equal("Result : ERROR: " + missingVar + " is undefined", result.Result);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("e.first")]
        [InlineData("e[1]")]
        [InlineData("d.x")]
        [InlineData("d[x]")]
        public void It_Should_Not_Display_An_Error_When_Values_Are_Missing(String varname)
        {
            // Arrange
            //Console.WriteLine(varname);
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenVariableMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection(new[] { LiquidValue.None, LiquidValue.None, LiquidValue.None }));
            ctx.DefineLocalVariable("d", new LiquidHash()
            {
                { "x", LiquidValue.None }
            });
            ctx.DefineLocalVariable("x", LiquidValue.None);

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.Equal("Result : ", result);
        }
    }
}
