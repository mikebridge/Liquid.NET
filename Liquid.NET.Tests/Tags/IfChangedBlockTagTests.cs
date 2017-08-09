using System;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class IfChangedBlockTagTests
    {
        [Fact]
        public void It_Should_Render_If_Changed()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign x = \"one|two|two|three\" | split: \"|\" %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("onetwothree", result);

        }

        [Fact]
        public void Two_Tags_Should_Not_Conflict()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign x = \"two|two|two|two\" | split: \"|\" %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("twotwo", result);

        }

    }
}
