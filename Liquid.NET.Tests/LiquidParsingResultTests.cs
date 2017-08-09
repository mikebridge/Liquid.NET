using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class LiquidParsingResultTests
    {
        [Fact]
        public void It_Should_Call_An_Error_Function()
        {
            // Arrange
            IList<LiquidError> errors = new List<LiquidError>{ new LiquidError { Message = "Hello" } };
            var parsingResult = LiquidParsingResult.Create(new LiquidTemplate(new LiquidAST()), errors);

            // Act
            IList<LiquidError> heardErrors = new List<LiquidError>();
            parsingResult.OnParsingError(heardErrors.Add);

            // Assert
            Assert.True(heardErrors.Any());

        }


    }
}
