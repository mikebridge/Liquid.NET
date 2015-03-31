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
    public class AssignTagTests
    {
        [Test]
        public void It_Should_Store_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = \"bar\" %}{{ foo }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("bar"));

        }
        [Test]
        public void It_Refer_To_Another_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = array %}{{ foo[1] }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("123"));

        }

        [Test]
        public void It_Should_Evaluate_An_Expresson()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = \"test\" | upcase %}{{ foo }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("TEST"));

        }

        private ArrayValue CreateArrayValues()
        {
            var list = new List<IExpressionConstant>
            {
                new StringValue("a string"),
                new NumericValue(123),
                new NumericValue(456m),
                new BooleanValue(false)
            };
            return new ArrayValue(list);
        }

    }
}
