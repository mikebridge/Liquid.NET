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

            ArrayValue arrayValue = new ArrayValue{
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(4));

        }

        [Test]
        public void An_Array_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue();
            var filter = new SizeFilter();
            
            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Measure_The_Size_Of_A_Dictionary()
        {
            // Arrange

            DictionaryValue dictValue = new DictionaryValue {
                {"string1", new StringValue("a string")},
                {"string2", NumericValue.Create(123)},
                {"string3", NumericValue.Create(456m)}
            };
            SizeFilter sizeFilter = new SizeFilter();

            // Act
            var result = sizeFilter.Apply(new TemplateContext(), dictValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(3));

        }

        [Test]
        public void A_Dict_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            DictionaryValue dictValue = new DictionaryValue();
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), dictValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Measure_The_Size_Of_A_String()
        {
            // Arrange
            var strVal = new StringValue("1234567890");
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(strVal.StringVal.Length));

        }

        [Test]
        public void A_String_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var strVal = new StringValue(null);
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void An_Undefined_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var filter = new SizeFilter();

            // Act
            var result = filter.ApplyToNil(new TemplateContext()).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void A_Generator_Value_Should_Return_The_Size()
        {
            // Arrange
            var strVal = new GeneratorValue(NumericValue.Create(3), NumericValue.Create(10));
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(8));

        }


    }
}
