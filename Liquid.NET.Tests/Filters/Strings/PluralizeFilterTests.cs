using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class PluralizeFilterTests
    {
        [Theory]
        [InlineData(1.2, "things")]
        public void It_Should_Pluralize_A_Decimal_Number(decimal input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", LiquidNumeric.Create(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.Equal("Result : "+ expected, result);

        }

        [Theory]
        [InlineData(2, "things")]
        [InlineData(1, "thing")]
        [InlineData(0, "things")]
        public void It_Should_Pluralize_An_Integerr(int input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", LiquidNumeric.Create(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.Equal("Result : " + expected, result);

        }

        [Theory]
        [InlineData("2", "things")]
        [InlineData("1", "thing")]
        [InlineData("1.2", "things")]
        [InlineData("0", "things")]
        [InlineData("z", "things")] // I  think this is what should happen...?
        public void It_Should_Pluralize_A_String(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", LiquidString.Create(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.Equal("Result : " + expected, result);

        }

        [Fact]
        public void It_Should_Return_The_String_When_Insufficient_Args()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", LiquidString.Create("1"));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize }}", ctx);

            // Assert
            Assert.Equal("Result : 1" , result);

        }

        [Fact]
        public void It_Should_Ignore_Missing_Plural()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", LiquidString.Create("1"));
            var result = RenderingHelper.RenderTemplate("Result : {{input}} {{ input | pluralize: 'thing' }}", ctx);

            // Assert
            Assert.Equal("Result : 1 thing", result);

        }

        [Fact]
        public void It_Should_Return_Zero_When_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", Option<ILiquidValue>.None());
            var result = RenderingHelper.RenderTemplate("Result : {{ input | default: 0 }} {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.Equal("Result : 0 things", result);

        } 

    }
}
