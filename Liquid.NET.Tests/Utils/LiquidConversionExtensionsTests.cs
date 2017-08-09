using System;
using System.Linq;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class LiquidConversionExtensionsTests
    {
        [Fact]
        public void It_Should_Extend_An_Object()
        {
            // Arrange
            var obj = new LiquidValueConverterTests.ClassWithAttributes { Ok = "To Liquid" };
            var templateContext = new TemplateContext()
                .ErrorWhenValueMissing()
                .DefineLocalVariable("test", obj.ToLiquid());

            // Act
            var parserResult = LiquidTemplate.Create("Hello {{ test.ok }}");
            Assert.False(parserResult.HasParsingErrors);
            var renderingResult = parserResult.LiquidTemplate.Render(templateContext);
            Assert.False(renderingResult.HasParsingErrors);
            Console.WriteLine(String.Join(",", renderingResult.RenderingErrors.Select(x => x.Message)));
            Assert.False(renderingResult.HasRenderingErrors);

            // Assert
            Assert.Equal("Hello To Liquid", renderingResult.Result);

        }
    }
}
