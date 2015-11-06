using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidStringTests
    {
        [Test]
        public void It_Should_Store_The_Value()
        {
            // Arrange
            var stringSymbol = new LiquidString("String Test");
            var result = stringSymbol.Value;

            // Assert
            Assert.That(result, Is.EqualTo("String Test"));

        }

        [Test]
        public void It_Should_Store_Null_As_Null()
        {
            // Arrange
            var stringSymbol = new LiquidString(null);
            var result = stringSymbol.Value;

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public void It_Should_Eval_A_Null_Value()
        {
            // Arrange
            var stringSymbol = new LiquidString(null);
            var result = stringSymbol.Eval(new TemplateContext(), new List<Option<ILiquidValue>>());

            // Assert
            Assert.That(result.SuccessValue<LiquidString>(), Is.EqualTo(stringSymbol));

        }

        [Test]
        public void It_ShouldJ_Join_Two_Values()
        {
            // Arrange
            var stringSymbol = new LiquidString("Hello");
            
            // Act
            LiquidString result = stringSymbol.Join(new LiquidString("World"));

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("HelloWorld"));

        }

     

    }
}
