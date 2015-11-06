using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidBooleanTests
    {
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void It_Should_Store_The_Value(bool val)
        {
            // Arrange
            var booleanSymbol = new LiquidBoolean(val);
            var result = booleanSymbol.Value;

            // Assert
            Assert.That(result, Is.EqualTo(val));

        }
    }
}
