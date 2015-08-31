using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
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
                new PlusFilter(new NumericValue(123)),
            };

            // Act
            var castedFilters = FilterChain.InterpolateCastFilters(filters).ToList();

            // Assert
            Assert.That(castedFilters.Count(), Is.EqualTo(3));
            Assert.That(castedFilters[1], Is.TypeOf(typeof(CastFilter<StringValue, NumericValue>)));

        }

        [Test]
        public void It_Should_Compose_Functions_Together()
        {
            // Arrange
            var removeFilter = new RemoveFilter(new StringValue("123"));
            var upCase = new UpCaseFilter();
            var filterList = new List<IFilterExpression> {removeFilter, upCase};
            var compositeFilterFn = FilterChain.CreateChain(new TemplateContext(), filterList);

            // Act
            var result = compositeFilterFn(new StringValue("test123")).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("TEST"));

        }

        [Test]
        public void It_Should_Allow_An_Int_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("bar", new NumericValue(3) );
            var template = LiquidTemplate.Create("{{ 1 | plus: bar }}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("4"));
        }

        [Test]
        public void It_Should_Allow_A_Variable_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            DictionaryValue dict = new DictionaryValue(new Dictionary<String, IExpressionConstant> { { "foo", new NumericValue(33) } });
            ctx.DefineLocalVariable("bar", dict);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar.foo}}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("34"));
        }

        [Test]
        public void It_Should_Allow_A_Variable_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx =
                new TemplateContext().WithAllFilters();
            ArrayValue arr = new ArrayValue(new List<IExpressionConstant> { new NumericValue(33) });
            ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create("{{ 1 | plus: bar[0]  }}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("34"));
        }

        [Test]
        public void It_Should_Parse_Two_Filter_Arguments()
        {
            // Arrange
            DictionaryValue dict = new DictionaryValue(new Dictionary<String, IExpressionConstant> { { "foo", new NumericValue(33)}, {"bar", new NumericValue(34) } });

            ITemplateContext ctx =
                new TemplateContext().WithAllFilters()
                    .WithFilter<FilterFactoryTests.MockStringToStringFilter>("mockfilter")
                    .DefineLocalVariable("bar", dict)
                    .DefineLocalVariable("foo", dict);
            ArrayValue arr = new ArrayValue(new List<IExpressionConstant> { new NumericValue(33) });
            ctx.DefineLocalVariable("bar", arr);
            var template = LiquidTemplate.Create("{{ 1 | mockfilter: bar.foo, foo.bar  }}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("1 33 34"));
        }

        [Test]
        public void It_Should_Allow_A_Variable()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();            
            ctx.DefineLocalVariable("a", new StringValue("HELLO"));
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a }} WORLD");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }

        [Test]
        public void It_Should_Lookup_An_Array_With_Int_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("a", new ArrayValue(new List<IExpressionConstant> {new StringValue("HELLO")}));
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a[0] }} WORLD");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }

        [Test]
        public void It_Should_Lookup_An_Array_In_A_Dictionary()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var arr = new ArrayValue(new List<IExpressionConstant> {new StringValue("HELLO")});
            ctx.DefineLocalVariable("a", new DictionaryValue(new Dictionary<String, IExpressionConstant> {{ "b", arr }}));
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ a.b[0] }} WORLD");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("HELLO WORLD"));

        }



    }
}
