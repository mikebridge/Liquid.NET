using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class SizeFilterTests
    {
        [Test]
        public void It_Should_Measure_The_Size_Of_An_Array()
        {
            // Arrange

            LiquidCollection liquidCollection = new LiquidCollection{
                new LiquidString("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(4));

        }

        [Test]
        public void An_Array_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection();
            var filter = new SizeFilter();
            
            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Measure_The_Size_Of_A_Dictionary()
        {
            // Arrange

            LiquidHash dictValue = new LiquidHash {
                {"string1", new LiquidString("a string")},
                {"string2", LiquidNumeric.Create(123)},
                {"string3", LiquidNumeric.Create(456m)}
            };
            SizeFilter sizeFilter = new SizeFilter();

            // Act
            var result = sizeFilter.Apply(new TemplateContext(), dictValue).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(3));

        }

        [Test]
        public void A_Dict_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            LiquidHash dictValue = new LiquidHash();
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), dictValue).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Measure_The_Size_Of_A_String()
        {
            // Arrange
            var strVal = new LiquidString("1234567890");
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(strVal.StringVal.Length));

        }

        [Test]
        public void A_String_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var strVal = new LiquidString(null);
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void An_Undefined_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var filter = new SizeFilter();

            // Act
            var result = filter.ApplyToNil(new TemplateContext()).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void A_Generator_Value_Should_Return_The_Size()
        {
            // Arrange
            var strVal = new LiquidRange(LiquidNumeric.Create(3), LiquidNumeric.Create(10));
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(8));

        }


    }
}
