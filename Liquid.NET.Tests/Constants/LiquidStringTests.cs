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
            var stringSymbol = LiquidString.Create("String Test");
            var result = stringSymbol.Value;

            // Assert
            Assert.That(result, Is.EqualTo("String Test"));

        }

        [Test]
        public void It_Should_Not_Allow_Null_Creation()
        {
            // Arrange
            var stringSymbol = LiquidString.Create(null);

            // Assert
            Assert.That(stringSymbol, Is.Null);

        }

//        [Test]
//        public void It_Should_Eval_A_Null_Value()
//        {
//            // Arrange
//            var stringSymbol = LiquidString.Create(null);
//            var result = stringSymbol.Eval(new TemplateContext(), new List<Option<ILiquidValue>>());
//
//            // Assert
//            Assert.That(result.SuccessValue<LiquidString>(), Is.EqualTo(stringSymbol));
//
//        }

        [Test]
        public void It_ShouldJ_Join_Two_Values()
        {
            // Arrange
            var stringSymbol = LiquidString.Create("Hello");
            
            // Act
            LiquidString result = stringSymbol.Join(LiquidString.Create("World"));

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("HelloWorld"));

        }

     

    }
}
