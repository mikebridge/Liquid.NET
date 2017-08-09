using System;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class IndexDereferencerTests
    {
        [Theory]
        [InlineData("3", "false")]
        [InlineData("4", "")]
        [InlineData("30", "")]
        [InlineData("-1", "false")]
        [InlineData("-2", "456.0")]
        [InlineData("-4", "a string")]
        [InlineData("-5", "")]
        [InlineData("4", "")]
        [InlineData("-30", "")]
        public void It_Should_Look_Up_ArrayValues(String index, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var liquidCollection = DataFixtures.CreateArrayValue();
            ctx.DefineLocalVariable("myarray", liquidCollection);
            String tmpl = "{{ myarray["+index+"] }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            //var expected = ValueCaster.RenderAsString(liquidCollection.ArrValue[3]);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("mydict.field1", "Property 1")]
        //[InlineData("mydict | lookup: \"field1\"", "Property 1")]
        [InlineData("mydict[\"field1\"]", "Property 1")]
        [InlineData("mydict[\"qwefqwefwef\"]", "")]
        public void It_Should_Look_Up_DictionaryValues(String liquid, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var dictValue = DataFixtures.CreateDictionary(1, "Property 1", "prop2");

            ctx.DefineLocalVariable("mydict", dictValue);
            String tmpl = "{{ " + liquid + " }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);
            Logger.Log(result);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("{% assign str = \"A String\" %}{{str[0]}}", "A")]
        [InlineData("{% assign str = \"A String\" %}{{str[-1]}}", "g")]
        [InlineData("{% assign str = \"A String\" %}{{str[-20]}}", "")]
        [InlineData("{% assign str = \"\" %}{{str[0]}}", "")]
        public void It_Should_Look_Up_StringValues(String input, String expected)
        {

            // Act
            var result = RenderingHelper.RenderTemplate(input);

            // Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        // see: https://github.com/Shopify/liquid/issues/543
        [InlineData("first", "a string")]
        [InlineData("last", "false")]
        [InlineData("size", "4")]
        public void It_Should_Dereference_Properties_Of_Array(String property, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var liquidCollection = DataFixtures.CreateArrayValue();
            ctx.DefineLocalVariable("myarray", liquidCollection);
            String tmpl = "{{ myarray." + property + " }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert          
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("first", "a")]
        [InlineData("last", "g")]
        [InlineData("size", "7")]
        public void It_Should_Dereference_Properties_Of_String(String property, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            //var liquidCollection = DataFixtures.CreateArrayValue();
            ctx.DefineLocalVariable("mystr", LiquidString.Create("abcdefg"));
            String tmpl = "{{ mystr." + property + " }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert          
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("size")]
        public void It_Should_Dereference_Properties_Of_Dictionary(String property)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var dictValue = DataFixtures.CreateDictionary(1, "Property 1", "prop2");
            ctx.DefineLocalVariable("mydict", dictValue);
            String tmpl = "{{ mydict." + property + " }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Assert.Equal(dictValue.Count.ToString(), result);
        }

        public LiquidCollection CreateArrayOfDicts()
        {
            // Arrange
            return new LiquidCollection{
                DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Value 2 A", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "Value 3 A", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "Value 4 A", "Value 4 B"),
            };

        }

    }
}
