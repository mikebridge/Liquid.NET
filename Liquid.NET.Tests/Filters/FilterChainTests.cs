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
            var compositeFilterFn = FilterChain.CreateChain(filterList);

            // Act
            var result = compositeFilterFn(new StringValue("test123"));

            // Assert
            Assert.That(result.Value, Is.EqualTo("TEST"));

        }


    }
}
