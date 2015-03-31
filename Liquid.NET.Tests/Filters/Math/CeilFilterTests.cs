using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class CeilFilterTests
    {
        [Test]
        [TestCase("123.4", 124)]
        [TestCase("123", 123)]
        [TestCase("124.4", 125)]
        [TestCase("-1.3", -1)]
        [TestCase("-1.5", -1)]
        public void It_Should_Find_The_Ceil_Value(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | ceil }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

    }
}
