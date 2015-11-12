using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class LiquidParsingResultTests
    {
        [Test]
        public void It_Should_Call_An_Error_Function()
        {
            // Arrange
            IList<LiquidError> errors = new List<LiquidError>{ new LiquidError { Message = "Hello" } };
            var parsingResult = LiquidParsingResult.Create(new LiquidTemplate(new LiquidAST()), errors);

            // Act
            IList<LiquidError> heardErrors = new List<LiquidError>();
            parsingResult.OnParsingError(heardErrors.Add);

            // Assert
            Assert.That(heardErrors.Any());

        }


    }
}
