using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CaptureBlockTests
    {
        [Test]
        public void It_Should_Put_A_Block_In_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = \"test\" | upcase %}{% capture cap %}Result: {{ foo }}{% endcapture %}Captured: {{ cap }}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Captured: Result: TEST"));

        }

    }
}
