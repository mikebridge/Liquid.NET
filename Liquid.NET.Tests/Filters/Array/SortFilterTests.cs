using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class SortFilterTests
    {
        [Test]
        public void It_Should_Sort_An_Array_By_StringValues()
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
            var filter = new SortFilter(new StringValue(""));

            // Act            
            var result = filter.Apply(arrayValue);
            var resultStrings = result.SuccessValue<ArrayValue>().Select(ValueCaster.RenderAsString);
            
            // Assert
            Assert.That(resultStrings, Is.EqualTo(new List<String>{"123", "456.0", "a string", "false"}));

        }


        [Test]
        public void It_Should_Sort_Dictionaries_By_Field()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                DataFixtures.CreateDictionary(1, "Aa", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Z", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "ab", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "b", "Value 4 B"),
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            SortFilter sizeFilter = new SortFilter(new StringValue("field1"));

            // Act
            var result = sizeFilter.Apply(arrayValue);

            // Assert
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 0, "field1").Value, Is.EqualTo("Aa"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 1, "field1").Value, Is.EqualTo("ab"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 2, "field1").Value, Is.EqualTo("b"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 3, "field1").Value, Is.EqualTo("Z"));

        }

        private static IExpressionConstant IdAt(ArrayValue result, int index, String field)
        {
            return ((DictionaryValue)result.ArrValue[index].Value).DictValue[field].Value;
        }
    }
}
