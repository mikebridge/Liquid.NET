using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class NumericValueTests
    {
        [Test]
        public void It_Should_Evaluate_To_Itself()
        {
            // Arrange
            var number = new NumericValue(123m);

            // Act
            var result = number.Eval(new SymbolTableStack(new TemplateContext()), new List<IExpressionConstant>());

            // Assert
            Assert.That(result.Value, Is.EqualTo(123m));

        }

        [Test]
        [TestCase(123.4, 123)]
        [TestCase(123.5, 124)]
        [TestCase(124.4, 124)]
        [TestCase(124.5, 125)] // Convert.ToInt32 rounds to nearest even #.
        [TestCase(-1.99, -2)]
        [TestCase(-1.4, -1)]
        [TestCase(0, 0)]
        public void It_Should_Round_A_Decimal_To_Nearest_Int(decimal input, int expected)
        {
            // Arrange
            var number = new NumericValue(input);

            // Assert
            Assert.That(number.IntValue, Is.EqualTo(expected));

        }

    }
}
