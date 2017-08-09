using System;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class IncrementTagTests
    {
        [Fact]
        public void It_Should_Increment_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var parsingResult = LiquidTemplate.Create("{% increment varname %}{% increment varname %}");
            Assert.False(parsingResult.HasParsingErrors);
            // Act
            String result = parsingResult.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("01", result);

        }

        [Fact]
        public void It_Should_Not_Interfere_With_An_Assign()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign varname = 9 %}{% increment varname %}{% increment varname %}{{ varname }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("019", result);

        }

        [Fact]
        public void It_Should_Not_Interfere_With_Another_Increment()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% increment varname %}{% increment varname2 %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("00", result);

        }

        [Fact]
        public void It_Should_Increment_And_Decrement_The_Same_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% increment varname %}{% decrement varname %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("00", result);

        }


    }
}
