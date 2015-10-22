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
            var result = mapFilter.Apply(new TemplateContext(), array).SuccessValue<ArrayValue>();

            // Assert
            var dictionaryValues = array.ArrValue.Select(x => x.Value).Cast<DictionaryValue>();

            IEnumerable<String> expected = dictionaryValues.Select(x => x.DictValue[field].Value.Value.ToString());
            //var expected = array.ArrValue.Cast<DictionaryValue>().Select(x => x.DictValue[field].Value.ToString());
            IEnumerable<String> actual = result.Select(x => x.Value.Value.ToString());
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void It_Should_Return_None_Where_A_Field_Is_Missing()
        {
            // Arrange
            var array = CreateArray();
            var field = "field2";

            var dictionaryValues = array.ArrValue.Select(x => x.Value).Cast<DictionaryValue>().ToList();
            dictionaryValues[1].DictValue.Remove(field);
            //array.ArrValue[1].Value.ValueAs<DictionaryValue>().DictValue.Remove(field);
            //((DictionaryValue) array.ArrValue[1]).DictValue.Remove(field);
            var mapFilter = new MapFilter(new StringValue(field));

            // Act
            var result = (mapFilter.Apply(new TemplateContext(), array).SuccessValue<ArrayValue>()).ToList();


            // Assert
            //IEnumerable<String> expected = dictionaryValues.Select(x => x.DictValue[field].;
//            var expected = array.ArrValue.Select(x => x.Value.ValueAs<DictionaryValue>().DictValue)
//                .Select(x => x.ContainsKey(field) ? x[field].Value.ToString() : "");
                

//            var expected = array.ArrValue.Cast<DictionaryValue>().Select(
//                x => x.DictValue.ContainsKey(field) ?
//                    x.DictValue[field].Value.ToString() :
//                    UndefinedMessage(field)).ToList();

            //Logger.Log("EXPECTED: " + String.Join(",", expected));
            Assert.That(result.Count(x => !x.HasValue), Is.EqualTo(1));
            //var actual = result.Select(x => x.Value.ToString());

            //Logger.Log("ACTUAL: " + String.Join(",", actual));
            //Assert.That(actual, Is.EquivalentTo(expected));

        }

        [Test]
        public void It_Should_Return_An_Error_When_Trying_To_Map_A_Non_Dictionary()
        {
            // Arrange
            var mapFilter = new MapFilter(new StringValue("field1"));
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                NumericValue.Create(123),
                new StringValue("Test")
            };
            // Act
            var result = mapFilter.Apply(new TemplateContext(), new ArrayValue(objlist)).SuccessValue<ArrayValue>();

            // Assert
            Assert.That(result.ArrValue.Count, Is.EqualTo(objlist.Count()));
            Assert.That(result.ArrValue[0].HasValue, Is.False);
            Assert.That(result.ArrValue[1].HasValue, Is.False);
        }

        [Test]
        public void It_Should_Do_The_Same_As_Lookup_When_Dictionary()
        {
            // Arrange
            var dict = DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B");
            var field = "field1";
            var mapFilter = new MapFilter(new StringValue(field));

            // Act
            var result = mapFilter.Apply(new TemplateContext(), dict).SuccessValue<StringValue>();

            // Assert
            Assert.That(result, Is.EquivalentTo("Value 1 A"));
        }

//        private static string UndefinedMessage(string field)
//        {
//            return Undefined.CreateUndefinedMessage(field).ToString();
//        }

        public ArrayValue CreateArray()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Value 2 A", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "Value 3 A", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "Value 4 A", "Value 4 B"),
            };
            return new ArrayValue(objlist);

        }
    }
}
