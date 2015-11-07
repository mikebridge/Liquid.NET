using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class FilterChainTests
    {
        [Test]
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
            Assert.That(castedFilters.Count, Is.EqualTo(3));
            Assert.That(castedFilters[1], Is.TypeOf(typeof(CastFilter<LiquidString, LiquidNumeric>)));

        }

        [Test]
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
            Assert.That(result.Value, Is.EqualTo("TEST"));

        }

        [Test]
        public void It_Should_Allow_An_Int_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("bar", LiquidNumeric.Create(3) );
            var template = LiquidTemplate.Create("{{ 1 | plus: bar }}");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("4"));
        }

        [Test]
        public void It_Should_Allow_A_Variable_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            LiquidHash dict = new LiquidHash { { "foo", LiquidNumeric.Create(33) } };
            ctx.DefineLocalVariable("bar", dict);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar.foo}}");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("34"));
        }

        [Test]
        public void It_Should_Allow_A_Variable_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx =
                new TemplateContext().WithAllFilters();
            LiquidCollection arr = new LiquidCollection { LiquidNumeric.Create(33) };
            ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar[0]  }}");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("34"));
        }

        [Test]
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
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("1 22 23"));
        }

        [Test]
        [TestCase("{{ 1 | mockfilter: bar.foo, 'X' }}", "1 22 X")]
        [TestCase("{{ 1 | mockfilter: 'X', bar.foo }}", "1 X 22")]
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
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void It_Should_Allow_A_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();            
            ctx.DefineLocalVariable("a", LiquidString.Create("HELLO"));
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a }} WORLD");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }

        [Test]
        public void It_Should_Lookup_An_Array_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("a", new LiquidCollection{LiquidString.Create("HELLO")});
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a[0] }} WORLD");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }

        [Test]
        public void It_Should_Lookup_An_Array_In_A_Dictionary()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var arr = new LiquidCollection{LiquidString.Create("HELLO")};
            ctx.DefineLocalVariable("a", new LiquidHash{{ "b", arr }});
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a.b[0] }} WORLD");

            // Act
            String result = template.Render(ctx);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }



    }
}
