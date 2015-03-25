using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
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
            var result = stringSymbol.Eval(new SymbolTableStack(new TemplateContext()), new List<IExpressionConstant>());

            // Assert
            Assert.That(result, Is.EqualTo(stringSymbol));

        }

        [Test]
        public void String_Should_Be_Enumerable()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail("Not Implemented Yet");

        }

    }
}
