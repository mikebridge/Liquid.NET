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
            var str1 = new LiquidString("hello");
            var str2 = new LiquidString("hello");
            
            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.True);

        }

        [Test]
        public void It_Should_Compare_Two_Unequal_Values()
        {
            // Arrange
            var str1 = new LiquidString("hello X");
            var str2 = new LiquidString("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.False);

        }

        [Test]
        public void It_Should_Compare_Two_Identical_Values()
        {
            // Arrange
            var str = new LiquidString("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(str, str), Is.True);

        }
        [Test]
        public void It_Should_Compare_Against_Null()
        {
            // Arrange
            var str = new LiquidString("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(null, str), Is.False);

        }
    }
}
