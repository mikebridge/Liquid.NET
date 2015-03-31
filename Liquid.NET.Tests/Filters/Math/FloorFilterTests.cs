using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class FloorFilterTests
    {
        [Test]
        [TestCase("123.4", 123)]
        [TestCase("123", 123)]
        [TestCase("124.4", 124)]
        [TestCase("-1.3", -2)]
        [TestCase("-1.5", -2)]
        public void It_Should_Find_The_Floor_Value(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | floor }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

    }
}
