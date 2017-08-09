using System.Collections.Generic;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class LiquidRenderingResultTests
    {

        [Fact]
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
            Assert.Equal(1, heardParsingErrors.Count);
            Assert.Equal(1, heardRenderingErrors.Count);
            Assert.Equal(2, heardAllErrors.Count);

        }

    }
}
