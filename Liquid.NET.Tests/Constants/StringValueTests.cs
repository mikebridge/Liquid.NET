using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class StringValueTests
    {
        [Test]
        public void It_Should_Store_The_Value()
        {
            // Arrange
            var stringSymbol = new StringValue("String Test");
            var result = stringSymbol.Value;

            // Assert
            Assert.That(result, Is.EqualTo("String Test"));

        }

        [Test]
        public void It_Should_Store_Null_As_Null()
        {
            // Arrange
            var stringSymbol = new StringValue(null);
            var result = stringSymbol.Value;

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public void It_Should_Eval_A_Null_Value()
        {
            // Arrange
            var stringSymbol = new StringValue(null);
            var result = stringSymbol.Eval(StackHelper.CreateSymbolTableStack(), new List<IExpressionConstant>());

            // Assert
            Assert.That(result, Is.EqualTo(stringSymbol));

        }

        [Test]
        public void It_ShouldJ_Join_Two_Values()
        {
            // Arrange
            var stringSymbol = new StringValue("Hello");
            
            // Act
            StringValue result = stringSymbol.Join(new StringValue("World"));

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("HelloWorld"));

        }

     

    }
}
