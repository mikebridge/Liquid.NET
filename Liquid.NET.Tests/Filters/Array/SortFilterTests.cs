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
            ArrayValue arrayValue = new ArrayValue{
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            var filter = new SortFilter(new StringValue(""));

            // Act            
            var result = filter.Apply(new TemplateContext(), arrayValue);
            var resultStrings = result.SuccessValue<ArrayValue>().Select(ValueCaster.RenderAsString);
            
            // Assert
            Assert.That(resultStrings, Is.EqualTo(new List<String>{"123", "456.0", "a string", "false"}));

        }


        [Test]
        public void It_Should_Sort_Dictionaries_By_Field()
        {
            // Arrange
            SortFilter sizeFilter = new SortFilter(new StringValue("field1"));

            // Act
            var result = sizeFilter.Apply(new TemplateContext(), CreateObjList());

            // Assert
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 0, "field1").Value, Is.EqualTo("Aa"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 1, "field1").Value, Is.EqualTo("ab"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 2, "field1").Value, Is.EqualTo("b"));
            Assert.That(IdAt(result.SuccessValue<ArrayValue>(), 3, "field1").Value, Is.EqualTo("Z"));
        }

        [Test]
        public void It_Should_Sort_Dictionaries_By_Field_From_Template()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"field1\" %}{{ x | map: \"field1\" }}", ctx);

            // Assert            
            Assert.That(result, Is.EqualTo("Result : AaabbZ"));
        }

        [Test]
        public void It_Should_Ignore_Dictionaries_With_Missing_Fields()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"test\" %}{{ x | map: \"id\" }}", ctx);

            // Assert            
            Assert.That(result, Is.EqualTo("Result : 1234"));
        }

        [Test]
        public void It_Should_Error_With_Dictionaries_With_Missing_Fields_When_Errors_On()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext().ErrorWhenValueMissing()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"test\" %}", ctx);

            // Assert            
            Assert.That(result, Is.StringContaining("an array element is missing the field \'test\'"));
        }


        private ArrayValue CreateObjList()
        {
            return new ArrayValue
            {
                DataFixtures.CreateDictionary(1, "Aa", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Z", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "ab", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "b", "Value 4 B"),
            };
        }

        private static IExpressionConstant IdAt(ArrayValue result, int index, String field)
        {
            return ((DictionaryValue)result[index].Value)[field].Value;
        }
    }
}
