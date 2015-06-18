using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class MapFilterTests
    {
        [Test]
        public void It_Should_Extract_The_Property_From_Each_Element()
        {
            // Arrange
            var array = CreateArray();
            var field = "field1";
            var mapFilter = new MapFilter(new StringValue(field));

            // Act
            var result = (ArrayValue) mapFilter.Apply(array);

            // Assert
            var expected = array.ArrValue.Cast<DictionaryValue>().Select(x => x.DictValue[field].Value.ToString());
            var actual = result.Select(x => x.Value.ToString());
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void It_Should_Return_An_Unknown_Where_A_Field_Is_Missing()
        {
            // Arrange
            var array = CreateArray();
            var field = "field2";
            ((DictionaryValue)array.ArrValue[1]).DictValue.Remove(field);
            var mapFilter = new MapFilter(new StringValue(field));

            // Act
            var result = ( (ArrayValue) mapFilter.Apply(array)).ToList();

            // Assert
            var expected = array.ArrValue.Cast<DictionaryValue>().Select(
                x => x.DictValue.ContainsKey(field) ?
                    x.DictValue[field].Value.ToString() :
                    UndefinedMessage(field)).ToList();

            Console.WriteLine("EXPECTED: " + String.Join(",", expected));
            var actual = result.Select(x => x.Value.ToString());

            Console.WriteLine("ACTUAL: " + String.Join(",", actual));
            Assert.That(actual, Is.EquivalentTo(expected));

        }

        [Test]
        public void It_Should_Return_An_Error_When_Trying_To_Map_A_Non_Dictionary()
        {
            // Arrange
            var mapFilter = new MapFilter(new StringValue("field1"));
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new NumericValue(123),
                new StringValue("Test")
            };
            // Act
            var result = (ArrayValue) mapFilter.Apply(new ArrayValue(objlist));

            // Assert
            Assert.That(result.ArrValue.Count, Is.EqualTo(objlist.Count()));
            Assert.That(result.ArrValue[0].Value.ToString(), Is.EqualTo(UndefinedMessage("field1")));
            Assert.That(result.ArrValue[1].Value.ToString(), Is.EqualTo(UndefinedMessage("field1")));
        }

        [Test]
        public void It_Should_Do_The_Same_As_Lookup_When_Dictionary()
        {
            // Arrange
            var dict = DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B");
            var field = "field1";
            var mapFilter = new MapFilter(new StringValue(field));

            // Act
            var result = (StringValue) mapFilter.Apply(dict);

            // Assert
            Assert.That(result, Is.EquivalentTo("Value 1 A"));
        }

        private static string UndefinedMessage(string field)
        {
            return Undefined.CreateUndefinedMessage(field).ToString();
        }

        public ArrayValue CreateArray()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B"), DataFixtures.CreateDictionary(2, "Value 2 A", "Value 2 B"), DataFixtures.CreateDictionary(3, "Value 3 A", "Value 3 B"), DataFixtures.CreateDictionary(4, "Value 4 A", "Value 4 B"),
            };
            return new ArrayValue(objlist);

        }
    }
}
