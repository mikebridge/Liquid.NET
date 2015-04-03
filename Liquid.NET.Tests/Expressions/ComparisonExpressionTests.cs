using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class ComparisonExpressionTests
    {
        [Test]
        [TestCase("1", "<", "2", "TRUE")]
        [TestCase("2", "<", "1", "FALSE")]
        [TestCase("2", ">", "1", "TRUE")]
        [TestCase("1", ">", "2", "FALSE")]
        [TestCase("1", "==", "1", "TRUE")]
        [TestCase("2", "==", "1", "FALSE")]
        [TestCase("1", ">=", "1", "TRUE")]
        [TestCase("1", ">=", "2", "FALSE")]
        [TestCase("1", "<=", "1", "TRUE")]
        [TestCase("2", "<=", "1", "FALSE")]
        public void It_Should_Compare_Two_Args(String arg1, string op, String arg2, string expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% if "+arg1+ " "+op+" " + arg2 +" %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

        [Test]
        [TestCase("2", "__", "1", "mismatched input '_' expecting")]
        public void It_Should_Throw_An_Error_When_Comparison_Incorrect(String arg1, string op, String arg2, string expected)
        {
            // Act

            try
            {
                var result = RenderingHelper.RenderTemplate("Result : {% if " + arg1 + " " + op + " " + arg2 +
                                                            " %}TRUE{% else %}FALSE{% endif %}");
                Assert.Fail("Expected an error but none was thrown.");
            }
            catch (LiquidParserException ex)
            {
                var errors = ex.LiquidErrors.Select(x => x.ToString());
                Assert.That(errors.Count(x => x.Contains(expected)), Is.EqualTo(1), "Could not find error containing '" + expected+"'");
            }
            // Assert
            //Assert.Fail("Need to figure out error message here.");
            
            //Assert.That(result, Is.EqualTo("Result : " + "ERROR"));
        }

    }
}
