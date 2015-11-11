using System;
using System.Linq;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class LiquidConversionExtensionsTests
    {
        [Test]
        public void It_Should_Extend_An_Object()
        {
            // Arrange
            var obj = new LiquidValueConverterTests.ClassWithAttributes { Ok = "To Liquid" };
            var templateContext = new TemplateContext()
                .ErrorWhenValueMissing()
                .DefineLocalVariable("test", obj.ToLiquid());

            // Act
            var parserResult = LiquidTemplate.Create("Hello {{ test.ok }}");
            Assert.That(parserResult.HasParsingErrors, Is.False);
            var renderingResult = parserResult.LiquidTemplate.Render(templateContext);
            Assert.That(renderingResult.HasParsingErrors, Is.False);
            Console.WriteLine(String.Join(",", renderingResult.RenderingErrors.Select(x => x.Message)));
            Assert.That(renderingResult.HasRenderingErrors, Is.False);

            // Assert
            Assert.That(renderingResult.Result, Is.EqualTo("Hello To Liquid"));

        }
    }
}
