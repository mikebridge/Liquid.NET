using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class EasyValueComparerTests
    {
        [Test]
        public void It_Should_Compare_Two_Equal_Values()
        {
            // Arrange
            var str1 = LiquidString.Create("hello");
            var str2 = LiquidString.Create("hello");
            
            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.True);

        }

        [Test]
        public void It_Should_Compare_Two_Unequal_Values()
        {
            // Arrange
            var str1 = LiquidString.Create("hello X");
            var str2 = LiquidString.Create("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.False);

        }

        [Test]
        public void It_Should_Compare_Two_Identical_Values()
        {
            // Arrange
            var str = LiquidString.Create("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(str, str), Is.True);

        }
        [Test]
        public void It_Should_Compare_Against_Null()
        {
            // Arrange
            var str = LiquidString.Create("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(null, str), Is.False);

        }
    }
}
