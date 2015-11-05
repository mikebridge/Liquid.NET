using System;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class PluralizeFilterTests
    {
        [Test]
        [TestCase(1.2, "1.2 things")]
        public void It_Should_Pluralize_A_Decimal_Number(decimal input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", NumericValue.Create(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+ expected));

        }

        [TestCase(2, "2 things")]
        [TestCase(1, "1 thing")]
        [TestCase(1, "1 thing")]
        [TestCase(0, "0 things")]
        public void It_Should_Pluralize_An_Integerr(int input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", NumericValue.Create(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }
        [Test]
        [TestCase("2", "2 things")]
        [TestCase("1", "1 thing")]
        [TestCase("1", "1 thing")]
        [TestCase("1.2", "1.2 things")]
        [TestCase("0", "0 things")]
        [TestCase("z", "0 things")] // I  think this is what should happen...?
        public void It_Should_Pluralize_A_String(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", new StringValue(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Return_The_String_When_Insufficient_Args()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", new StringValue("1"));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1" ));

        }

        [Test]
        public void It_Should_Ignore_Missing_Plural()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", new StringValue("1"));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1 thing"));

        }

        [Test]
        public void It_Should_Return_Zero_When_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("input", null);
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 0 things"));

        } 

    }
}
