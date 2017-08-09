using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class FilterChainTests
    {
        [Fact]
        public void It_Should_Cast_MisMatched_Filters()
        {
            // Arrange
            var filters = new List<IFilterExpression>
            {
                new UpCaseFilter(),
                new PlusFilter(LiquidNumeric.Create(123)),
            };

            // Act
            var castedFilters = FilterChain.InterpolateCastFilters(filters).ToList();

            // Assert
            Assert.Equal(3, castedFilters.Count);
            //Assert.That(castedFilters[1], Is.TypeOf(typeof(CastFilter<LiquidString, LiquidNumeric>)));
            Assert.IsType< CastFilter<LiquidString, LiquidNumeric>>(castedFilters[1]);

        }

        [Fact]
        public void It_Should_Compose_Functions_Together()
        {
            // Arrange
            var removeFilter = new RemoveFilter(LiquidString.Create("123"));
            var upCase = new UpCaseFilter();
            var filterList = new List<IFilterExpression> {removeFilter, upCase};
            var compositeFilterFn = FilterChain.CreateChain(new TemplateContext(), filterList);

            // Act
            var result = compositeFilterFn(LiquidString.Create("test123")).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("TEST", result.Value);

        }

        [Fact]
        public void It_Should_Allow_An_Int_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("bar", LiquidNumeric.Create(3) );
            var template = LiquidTemplate.Create("{{ 1 | plus: bar }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("4", result);
        }

        [Fact]
        public void It_Should_Allow_A_Variable_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            LiquidHash dict = new LiquidHash { { "foo", LiquidNumeric.Create(33) } };
            ctx.DefineLocalVariable("bar", dict);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar.foo}}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("34", result);
        }

        [Fact]
        public void It_Should_Allow_A_Variable_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx =
                new TemplateContext().WithAllFilters();
            LiquidCollection arr = new LiquidCollection { LiquidNumeric.Create(33) };
            ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar[0]  }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("34", result);
        }

        [Fact]
        public void It_Should_Parse_Two_Filter_Arguments()
        {
            // Arrange
            LiquidHash dict = new LiquidHash{ { "foo", LiquidNumeric.Create(22)}, {"bar", LiquidNumeric.Create(23) } };

            ITemplateContext ctx =
                new TemplateContext().WithAllFilters()
                    .WithFilter<FilterFactoryTests.MockStringToStringFilter>("mockfilter")
                    .DefineLocalVariable("bar", dict)
                    .DefineLocalVariable("foo", dict);
            //LiquidCollection arr = new LiquidCollection(new List<ILiquidValue> { new LiquidNumeric(33) });
            //ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create("{{ 1 | mockfilter: bar.foo, foo.bar  }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("1 22 23", result);
        }

        [Theory]
        [InlineData("{{ 1 | mockfilter: bar.foo, 'X' }}", "1 22 X")]
        [InlineData("{{ 1 | mockfilter: 'X', bar.foo }}", "1 X 22")]
        public void It_Should_Parse_A_Variable_And_A_Value(String liquid, String expected)
        {
            // Arrange
            LiquidHash dict = new LiquidHash { { "foo", LiquidNumeric.Create(22) }, { "bar", LiquidNumeric.Create(23) } };

            ITemplateContext ctx =
                new TemplateContext().WithAllFilters()
                    .WithFilter<FilterFactoryTests.MockStringToStringFilter>("mockfilter")
                    .DefineLocalVariable("bar", dict)
                    .DefineLocalVariable("foo", dict);
            //LiquidCollection arr = new LiquidCollection(new List<ILiquidValue> { new LiquidNumeric(33) });
            //ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create(liquid);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal(expected, result);
        }


        [Fact]
        public void It_Should_Allow_A_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();            
            ctx.DefineLocalVariable("a", LiquidString.Create("HELLO"));
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a }} WORLD");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("HELLO WORLD", result);

        }

        [Fact]
        public void It_Should_Lookup_An_Array_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("a", new LiquidCollection{LiquidString.Create("HELLO")});
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a[0] }} WORLD");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("HELLO WORLD", result);

        }

        [Fact]
        public void It_Should_Lookup_An_Array_In_A_Dictionary()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var arr = new LiquidCollection{LiquidString.Create("HELLO")};
            ctx.DefineLocalVariable("a", new LiquidHash{{ "b", arr }});
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a.b[0] }} WORLD");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Equal("HELLO WORLD", result);

        }



    }
}
