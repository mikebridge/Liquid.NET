using System.Collections.Generic;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class LiquidRenderingResultTests
    {

        [Test]
        public void It_Should_Call_An_Error_Function()
        {
            // Arrange
            IList<LiquidError> renderingErrors = new List<LiquidError> { new LiquidError { Message = "Rendering" } };
            IList<LiquidError> parsingErrors = new List<LiquidError> { new LiquidError { Message = "Parsing" } };
            var parsingResult = LiquidRenderingResult.Create("result", renderingErrors, parsingErrors);

            // Act
            IList<LiquidError> heardParsingErrors = new List<LiquidError>();
            IList<LiquidError> heardRenderingErrors = new List<LiquidError>();
            IList<LiquidError> heardAllErrors = new List<LiquidError>();
            parsingResult.OnParsingError(heardParsingErrors.Add);
            parsingResult.OnRenderingError(heardRenderingErrors.Add);
            parsingResult.OnAnyError(heardAllErrors.Add);

            // Assert
            Assert.That(heardParsingErrors.Count, Is.EqualTo(1));
            Assert.That(heardRenderingErrors.Count, Is.EqualTo(1));
            Assert.That(heardAllErrors.Count, Is.EqualTo(2));

        }

    }
}
