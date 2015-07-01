﻿using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;
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
            var result = number.Eval(SymbolTableStackFactory.CreateSymbolTableStack(new TemplateContext()), new List<Option<IExpressionConstant>>()).SuccessValue<NumericValue>();

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

        [Test]
        [Ignore("Should this be changed?")]
        public void It_Should_Format_Without_Trailing_Zeroes()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 0.0075| times: 100 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 0.75"));
        }


        [Test]
        public void It_Should_Cast_A_Non_Numeric_String_To_Zero()
        {
            // Act
            NumericValue number = NumericValue.Parse("z").SuccessValue<NumericValue>();

            // Assert
            Assert.That(number.IntValue, Is.EqualTo(0));

        }

    }
}
