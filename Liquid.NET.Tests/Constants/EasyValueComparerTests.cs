using Liquid.NET.Constants;
using Xunit;


namespace Liquid.NET.Tests.Constants
{
    
    public class EasyValueComparerTests
    {
        [Fact]
        public void It_Should_Compare_Two_Equal_Values()
        {
            // Arrange
            var str1 = LiquidString.Create("hello");
            var str2 = LiquidString.Create("hello");
            
            // Act
            Assert.True(new EasyValueComparer().Equals(str1, str2));

        }

        [Fact]
        public void It_Should_Compare_Two_Unequal_Values()
        {
            // Arrange
            var str1 = LiquidString.Create("hello X");
            var str2 = LiquidString.Create("hello");

            // Act
            Assert.False(new EasyValueComparer().Equals(str1, str2));

        }

        [Fact]
        public void It_Should_Compare_Two_Identical_Values()
        {
            // Arrange
            var str = LiquidString.Create("hello");

            // Act
            Assert.True(new EasyValueComparer().Equals(str, str));

        }
        [Fact]
        public void It_Should_Compare_Against_Null()
        {
            // Arrange
            var str = LiquidString.Create("hello");

            // Act
            Assert.False(new EasyValueComparer().Equals(null, str));

        }
    }
}
