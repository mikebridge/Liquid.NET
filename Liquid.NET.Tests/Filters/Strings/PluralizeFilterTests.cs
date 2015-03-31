using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class PluralizeFilterTests
    {
        [Test]
        [TestCase(2, "2 things")]
        [TestCase(1, "1 thing")]
        [TestCase(1, "1 thing")]
        [TestCase(1.2, "1.2 things")]
        [TestCase(0, "0 things")]
        public void It_Should_Pluralize_A_Number(decimal input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("input", new NumericValue(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+ expected));

        }

        [Test]
        [TestCase("2", "2 things")]
        [TestCase("1", "1 thing")]
        [TestCase("1", "1 thing")]
        [TestCase("1.2", "1.2 things")]
        [TestCase("0", "0 things")]
        [TestCase("z", "")] // this should probably print nothing?
        public void It_Should_Pluralize_A_String(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("input", new StringValue(input));
            var result = RenderingHelper.RenderTemplate("Result : {{ input | pluralize: 'thing', 'things' }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        } 

    }
}
