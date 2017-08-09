using System;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class DecrementTagTests
    {
        [Fact]
        public void It_Should_Decrement_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% decrement varname %}{% decrement varname %}");
            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("-1-2", result);

        }

        [Fact]
        public void It_Should_Not_Interfere_With_An_Assign()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign varname = 9 %}{% decrement varname %}{% decrement varname %}{{ varname }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("-1-29", result);

        }

        [Fact]
        public void It_Should_Not_Interfere_With_Another_Decrement()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% decrement varname %}{% decrement varname2 %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("-1-1", result);

        }

    }
}
