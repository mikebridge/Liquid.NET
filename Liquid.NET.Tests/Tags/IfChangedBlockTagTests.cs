using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class IfChangedBlockTagTests
    {
        [Test]
        public void It_Should_Render_If_Changed()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign x = \"one|two|two|three\" | split: \"|\" %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("onetwothree"));

        }

        [Test]
        public void Two_Tags_Should_Not_Conflict()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign x = \"two|two|two|two\" | split: \"|\" %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}{% for i in x %}{% ifchanged %}{{ i }}{% endifchanged %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("twotwo"));

        }

    }
}
