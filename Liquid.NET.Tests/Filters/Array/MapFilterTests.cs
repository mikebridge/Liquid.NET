using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Filters.Array
{
    
    public class MapFilterTests
    {
        [Fact]
        public void It_Should_Extract_The_Property_From_Each_Element()
        {
            // Arrange
            var array = CreateArray();
            var field = "field1";
            var mapFilter = new MapFilter(LiquidString.Create(field));

            // Act
            var result = mapFilter.Apply(new TemplateContext(), array).SuccessValue<LiquidCollection>();

            // Assert
            var dictionaryValues = array.Select(x => x.Value).Cast<LiquidHash>();

            IEnumerable<String> expected = dictionaryValues.Select(x => x[field].Value.Value.ToString());
            //var expected = array.ArrValue.Cast<LiquidHash>().Select(x => x.DictValue[field].Value.ToString());
            IEnumerable<String> actual = result.Select(x => x.Value.Value.ToString());
            //Assert.That(actual, Is.EquivalentTo(expected));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void It_Should_Return_None_Where_A_Field_Is_Missing()
        {
            // Arrange
            var array = CreateArray();
            var field = "field2";

            var dictionaryValues = array.Select(x => x.Value).Cast<LiquidHash>().ToList();
            dictionaryValues[1].Remove(field);
            //array.ArrValue[1].Value.ValueAs<LiquidHash>().DictValue.Remove(field);
            //((LiquidHash) array.ArrValue[1]).DictValue.Remove(field);
            var mapFilter = new MapFilter(LiquidString.Create(field));

            // Act
            var result = (mapFilter.Apply(new TemplateContext(), array).SuccessValue<LiquidCollection>()).ToList();
            Assert.Equal(1, result.Count(x => !x.HasValue));

        }

        [Fact]
        public void It_Should_Return_An_Error_When_Trying_To_Map_A_Non_Dictionary()
        {
            // Arrange
            var mapFilter = new MapFilter(LiquidString.Create("field1"));
            var liquidCollection = new LiquidCollection
            {
                LiquidNumeric.Create(123),
                LiquidString.Create("Test")
            };
            // Act
            var result = mapFilter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidCollection>();

            // Assert
            Assert.Equal(liquidCollection.Count, result.Count);
            Assert.False(result[0].HasValue);
            Assert.False(result[1].HasValue);
        }

        [Fact]
        public void It_Should_Do_The_Same_As_Lookup_When_Dictionary()
        {
            // Arrange
            var dict = DataFixtures.CreateDictionary(1, "Value 1 A", "Value 1 B");
            var field = "field1";
            var mapFilter = new MapFilter(LiquidString.Create(field));

            // Act
            var result = mapFilter.Apply(new TemplateContext(), dict).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("Value 1 A", result.Value.ToString());
        }


        [Fact]
        public void It_Should_Render_The_Fields()
        {
            // Arrange
            var array = CreateArray();
            ITemplateContext ctx = new TemplateContext()             
                .WithAllFilters().DefineLocalVariable("arr", array);
            var result = RenderingHelper.RenderTemplate("Result : {{ arr | map: \"field1\" }}",ctx);

            // Assert
            Assert.Equal("Result : Value 1 AValue 2 AValue 3 AValue 4 A", result);
        }

        [Fact]
        public void It_Should_Render_Missing_Fields_When_ErrorsOff()
        {
            // Arrange
            var array = CreateArray();
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters().DefineLocalVariable("arr", array);
            var result = RenderingHelper.RenderTemplate("Result : {{ arr | map: \"awefwef\" }}", ctx);

            // Assert
            Assert.Equal("Result : ", result);
        }

        [Fact]
        public void It_Should_Error_When_No_Field_When_ErrorsOn()
        {
            // Arrange
            var array = CreateArray();
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing()
                .WithAllFilters().DefineLocalVariable("arr", array);

            //var result = RenderingHelper.RenderTemplate("Result : {{ arr | map: \"missing\" }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ arr | map: \"missing\" }}");
            var result = template.LiquidTemplate.Render(ctx);

            Console.WriteLine("Result "+result);
            // Assert
            Assert.Contains("missing is undefined", result.Result);
        }

        public LiquidCollection CreateArray()
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
