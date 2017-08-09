using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidValueTests
    {

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, true)]
        public void NumericValues_Should_Equal(decimal decvar1, decimal decvar2, bool expected)
        {
            // Arrange
            var var1 = LiquidNumeric.Create(decvar1);
            var var2 = LiquidNumeric.Create(decvar2);

            // Assert
            Assert.Equal(expected, var1.Equals(var2));

        }

        [Fact]
        public void NumericValues_Should_Not_Equal_Null()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);            

            // Assert
            Assert.NotNull(var1);

        }

        [Fact]
        public void ToString_Should_Render_As_String()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);

            // Assert
            Assert.Equal("1", var1.ToString());

        }

        [Fact]
        public void ToOption_Should_Convert_To_Some()
        {
            // Arrange
            var str = LiquidString.Create("test");

            // Act
            var option = str.ToOption();

            // Assert
            Assert.Equal(LiquidString.Create("test"), option.Value);

        }

    }
}
