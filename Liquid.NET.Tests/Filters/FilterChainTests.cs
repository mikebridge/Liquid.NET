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
        public void It_Should_Allow_A_Variable_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            DictionaryValue dict = new DictionaryValue(new Dictionary<String, IExpressionConstant> { { "foo", new NumericValue(33) } });
            ctx.DefineLocalVariable("bar", dict);
            //var template = LiquidTemplate.Create("{% assign x = 1 | plus: bar.foo %} 1 + {{ bar.foo }} = {{ x }}");
            var template = LiquidTemplate.Create("{{ 1 | plus: bar.foo }}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("2"));

        }

    }
}
