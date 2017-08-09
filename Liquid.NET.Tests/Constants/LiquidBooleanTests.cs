using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidBooleanTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void It_Should_Store_The_Value(bool val)
        {
            // Arrange
            var booleanSymbol = new LiquidBoolean(val);
            var result = booleanSymbol.Value;

            // Assert
            Assert.Equal(val, result);

        }
    }
}
