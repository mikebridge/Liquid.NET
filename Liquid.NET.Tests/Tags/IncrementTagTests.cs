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
    public class IncrementTagTests
    {
        [Test]
        public void It_Should_Increment_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% increment varname %}{% increment varname %}");
            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("01"));

        }

        [Test]
        public void It_Should_Not_Interfere_With_An_Assign()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign varname = 9 %}{% increment varname %}{% increment varname %}{{ varname }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("019"));

        }

        [Test]
        public void It_Should_Not_Interfere_With_Another_Increment()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% increment varname %}{% increment varname2 %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("00"));

        }

        [Test]
        public void It_Should_Increment_And_Decrement_The_Same_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% increment varname %}{% decrement varname %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("00"));

        }

        [Test]
        [Ignore("Not implemented: the index isn't stored a reusable place.")]
        public void Rendering_Repeatedly_Will_Use_The_Same_Context()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var tmpl = "{% increment a %}";
            var template = LiquidTemplate.Create(tmpl);
            var template2 = LiquidTemplate.Create(tmpl);

            // Act
            String result = template.Render(ctx);
            String result2 = template2.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("0"));
            Assert.That(result2, Is.EqualTo("1"));

        }


    }
}
