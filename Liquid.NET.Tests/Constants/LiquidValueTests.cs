using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidValueTests
    {

        [Test]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        public void NumericValues_Should_Equal(decimal decvar1, decimal decvar2, bool expected)
        {
            // Arrange
            var var1 = LiquidNumeric.Create(decvar1);
            var var2 = LiquidNumeric.Create(decvar2);

            // Assert
            Assert.That(var1.Equals(var2), Is.EqualTo(expected));

        }

        [Test]
        public void NumericValues_Should_Not_Equal_Null()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);            

            // Assert
            Assert.That(var1.Equals(null), Is.EqualTo(false));

        }

        [Test]
        public void ToString_Should_Render_As_String()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);

            // Assert
            Assert.That(var1.ToString(), Is.EqualTo("1"));

        }

        [Test]
        public void ToOption_Should_Convert_To_Some()
        {
            // Arrange
            var str = LiquidString.Create("test");

            // Act
            var option = str.ToOption();

            // Assert
            Assert.That(option.Value, Is.EqualTo(LiquidString.Create("test")));

        }

    }
}
