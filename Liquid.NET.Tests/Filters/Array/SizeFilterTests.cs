using System;
using System.Collections.Generic;
using System.Linq;

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
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("a string"), 
                new NumericValue(123), 
                new NumericValue(456m),
                new BooleanValue(false)
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(arrayValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(4));

        }

        [Test]
        public void An_Array_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue(new List<IExpressionConstant>());
            var filter = new SizeFilter();
            
            // Act
            var result = filter.Apply(arrayValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Measure_The_Size_Of_A_Dictionary()
        {
            // Arrange
            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"string1", new StringValue("a string")},
                {"string2", new NumericValue(123)},
                {"string3", new NumericValue(456m)}
            };
            DictionaryValue dictValue = new DictionaryValue(dict);
            SizeFilter sizeFilter = new SizeFilter();

            // Act
            var result = sizeFilter.Apply(dictValue).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(dict.Keys.Count()));

        }

        [Test]
        public void A_Dict_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            DictionaryValue dictValue = new DictionaryValue(new Dictionary<String,Option<IExpressionConstant>>());
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(dictValue).SuccessValue<NumericValue>();

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
            var result = filter.Apply(strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(strVal.StringVal.Count()));

        }

        [Test]
        public void A_String_With_No_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var strVal = new StringValue(null);
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void An_Undefined_Value_Should_Have_Zero_Length()
        {
            // Arrange
            var strVal = new None<IExpressionConstant>();
            var filter = new SizeFilter();

            // Act
            var result = filter.ApplyToNil().SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(0));

        }

        [Test]
        public void A_Generator_Value_Should_Return_The_Size()
        {
            // Arrange
            var strVal = new GeneratorValue(new NumericValue(3), new NumericValue(10));
            var filter = new SizeFilter();

            // Act
            var result = filter.Apply(strVal).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(8));

        }


    }
}
