using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CaptureBlockTagTests
    {
        [Test]
        public void It_Should_Put_A_Block_In_A_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign foo = \"test\" | upcase %}{% capture cap %}Result: {{ foo }}{% endcapture %}Captured: {{ cap }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Captured: Result: TEST"));

        }

        [Test]
        public void It_Should_Assign_A_Cycle_To_A_Capture()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}{% capture thecycle %}{% cycle 'odd', 'even' %}{% endcapture %}{{ thecycle }} {% endfor %}");

            // Act
            try
            {
                String result = template.Render(ctx);
                Logger.Log(result);

                // Assert
                Assert.That(result.TrimEnd(), Is.EqualTo("Result : odd even odd even"));
            }
            catch (LiquidRendererException ex)
            {
                Logger.Log(ex.Message);
                throw;
            }

        }

        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection
            {
                new LiquidString("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }

    }
}
