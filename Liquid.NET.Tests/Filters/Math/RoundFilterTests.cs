using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class RoundFilterTests
    {        
        [Test]
        [TestCase("123.4", 123)]
        [TestCase("123.5", 124)]
        [TestCase("124.4", 124)]
        [TestCase("124.5", 125)]
        [TestCase("-1.3", -1)]
        [TestCase("-1.5", -2)]
        public void It_Should_Round_To_Nearest_Whole_Number_Away_From_Zero(String input, int expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ "+input+" | round }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

        [Test]
        [TestCase("123.33333333333", 2, "123.33")]
        [TestCase("123.5", 0, "124")]
        [TestCase("124.45", 1, "124.5")]
        [TestCase("124.499", 1, "124.5")]
        [TestCase("-1.3", -1, "-1")]
        [TestCase("-1.5", 0, "-2")]
        [TestCase("1", null, "1")]
        public void It_Should_Round_To_Nearest_Decimal_Number(String input, int digits, String expected)
        {
            // Act
            //var result = LiquidHtmlFilters.ToInt(input);
            var result = RenderingHelper.RenderTemplate("Result : {{ " + input + " | round : "+digits+" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " +expected));
        }

    }
}
