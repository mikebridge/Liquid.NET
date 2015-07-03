using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Tests.Filters.Array;

using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class LookupFilterTests
    {
        private static String LOOKUP = "lookup";

        [Test]
        [TestCase("3", "false")]
        [TestCase("4", "")]
        [TestCase("30", "")]
        [TestCase("-1", "false")]
        [TestCase("-2", "456.0")]
        [TestCase("-4", "a string")]
        [TestCase("-5", "")]
        [TestCase("4", "")]
        [TestCase("-30", "")]
        public void It_Should_Look_Up_ArrayValues(String index, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var arrayValue = DataFixtures.CreateArrayValue();
            ctx.Define("myarray", arrayValue);
            String tmpl = "{{ myarray | " + LOOKUP + ": "+index+" }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            //var expected = ValueCaster.RenderAsString(arrayValue.ArrValue[3]);
            Assert.That(result, Is.EqualTo(expected));          
        }

        [Test]
        [TestCase("mydict.field1", "Property 1")]
        [TestCase("mydict | lookup: \"field1\"", "Property 1")]
        [TestCase("mydict[\"field1\"]", "Property 1")]
        [TestCase("mydict[\"qwefqwefwef\"]", "")]
        public void It_Should_Look_Up_DictionaryValues(String liquid, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var dictValue = DataFixtures.CreateDictionary(1, "Property 1", "prop2");
            ctx.Define("mydict", dictValue);
            String tmpl = "{{ "+liquid+" }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Assert.That(result, Is.EqualTo(expected));          
        }

        [Test]
        [TestCase("{% assign str = \"A String\" %}{{str[0]}}", "A")]
        [TestCase("{% assign str = \"A String\" %}{{str[-1]}}", "g")]
        [TestCase("{% assign str = \"A String\" %}{{str[-20]}}", "")]
        [TestCase("{% assign str = \"\" %}{{str[0]}}", "")]
        public void It_Should_Look_Up_StringValues(String input, String expected)
        {
           
            // Act
            var result = RenderingHelper.RenderTemplate(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        // see: https://github.com/Shopify/liquid/issues/543
        [TestCase("first", "a string")]
        [TestCase("last", "false")]
        [TestCase("size", "4")]
        public void It_Should_Dereference_Properties_Of_Array(String property, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var arrayValue = DataFixtures.CreateArrayValue();
            ctx.Define("myarray", arrayValue);
            String tmpl = "{{ myarray."+property+" }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert          
            Assert.That(result, Is.EqualTo(expected));          
        }

        [Test]
        [TestCase("size")]
        public void It_Should_Dereference_Properties_Of_Dictionary(String property)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var dictValue = DataFixtures.CreateDictionary(1, "Property 1", "prop2");
            ctx.Define("mydict", dictValue);
            String tmpl = "{{ mydict." + property + " }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Assert.That(result, Is.EqualTo(dictValue.DictValue.Count.ToString()));
        }

        public ArrayValue CreateArrayOfDicts()
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
