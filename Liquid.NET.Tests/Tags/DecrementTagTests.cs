using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class DecrementTagTests
    {
        [Test]
        public void It_Should_Decrement_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% decrement varname %}{% decrement varname %}");
            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("-1-2"));

        }

        [Test]
        public void It_Should_Not_Interfere_With_An_Assign()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign varname = 9 %}{% decrement varname %}{% decrement varname %}{{ varname }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("-1-29"));

        }

        [Test]
        public void It_Should_Not_Interfere_With_Another_Decrement()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% decrement varname %}{% decrement varname2 %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("-1-1"));

        }

    }
}
