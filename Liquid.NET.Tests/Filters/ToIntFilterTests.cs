using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class ToIntFilterTests
    {
        [Test]
        [TestCase("123", 123)]
        [TestCase("123.45", 123)]
        [TestCase("123.5", 124)]
        [TestCase("124.5", 125)]
        [TestCase("-124.5", -125)]
        [TestCase("-123.5", -124)]
        [TestCase("wefwef", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void It_Should_Cast_A_String_To_An_Int(String input, int expected)
        {
            // Arrange
            var toIntFilter = new ToIntFilter();

            // Act
            var result = toIntFilter.Apply(new TemplateContext(), LiquidString.Create(input)).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.DecimalValue, Is.EqualTo((decimal) expected));
            Assert.That(result.IntValue, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, 0)]
        [TestCase(true, 1)]
        public void It_Should_Cast_A_Bool_To_An_Int(bool val, int expected)
        {
            // Arrange
            var toIntFilter = new ToIntFilter();

            // Act
            var result = toIntFilter.Apply(new TemplateContext(), new LiquidBoolean(val)).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.DecimalValue, Is.EqualTo((decimal)expected));
            Assert.That(result.IntValue, Is.EqualTo(expected));

        }

       

    }
}
