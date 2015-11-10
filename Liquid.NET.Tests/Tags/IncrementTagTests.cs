using System;
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
            var parsingResult = LiquidTemplate.Create("{% increment varname %}{% increment varname %}");
            Assert.That(parsingResult.HasParsingErrors, Is.False);
            // Act
            String result = parsingResult.LiquidTemplate.Render(ctx).Result;

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
            String result = template.LiquidTemplate.Render(ctx).Result;

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
            String result = template.LiquidTemplate.Render(ctx).Result;

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
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("00"));

        }


    }
}
