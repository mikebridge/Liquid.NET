using System;
using System.Linq;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class ComparisonExpressionTests
    {
        [Theory]
        [InlineData("1", "<", "2", "TRUE")]
        [InlineData("2", "<", "1", "FALSE")]
        [InlineData("2", ">", "1", "TRUE")]
        [InlineData("1", ">", "2", "FALSE")]
        [InlineData("1", "==", "1", "TRUE")]
        [InlineData("2", "==", "1", "FALSE")]
        [InlineData("1", ">=", "1", "TRUE")]
        [InlineData("1", ">=", "2", "FALSE")]
        [InlineData("1", "<=", "1", "TRUE")]
        [InlineData("2", "<=", "1", "FALSE")]
        [InlineData("1", "==", "null", "FALSE")]
        [InlineData("null", "==", "1", "FALSE")]
        //[InlineData("null", "==", "null", "TRUE")]
        [InlineData("1", ">", "null", "FALSE")]
        [InlineData("1", "!=", "null", "TRUE")]
        [InlineData("1", "<", "null", "FALSE")]
        [InlineData("1", "<=", "null", "FALSE")]
        [InlineData("1", ">=", "null", "FALSE")]
        [InlineData("null", "!=", "null", "FALSE")]
        [InlineData("1", "!=", "1", "FALSE")]
        public void It_Should_Compare_Two_Args(String arg1, string op, String arg2, string expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% if "+arg1+ " "+op+" " + arg2 +" %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.Equal("Result : " + expected, result);
        }

        

        [Theory]
        [InlineData("2", "__", "1", "mismatched input '_' expecting")]
        public void It_Should_Throw_An_Error_When_Comparison_Incorrect(String arg1, string op, String arg2, string expected)
        {
            // Act

            //try
            //{
                //var result = RenderingHelper.RenderTemplate("Result : {% if " + arg1 + " " + op + " " + arg2 +
                //                                            " %}TRUE{% else %}FALSE{% endif %}");
                var template = LiquidTemplate.Create("Result : {% if " + arg1 + " " + op + " " + arg2 +
                                                            " %}TRUE{% else %}FALSE{% endif %}");
                //var result = template.LiquidTemplate.Render(new TemplateContext().WithAllFilters());

                var errors = template.ParsingErrors.Select(x => x.ToString());
            Assert.Equal(1,
                errors.Count(x => x.Contains(expected))); //"Could not find error containing '" + expected + "'";

            //L//ogger.Log(result.Result);
            //Assert.Fail("Expected an error but none was thrown: " + result);
            //}
            //catch (LiquidParserException ex)
            //{
            //    var errors = ex.LiquidErrors.Select(x => x.ToString());
            //    Assert.That(errors.Count(x => x.Contains(expected)), Is.EqualTo(1), "Could not find error containing '" + expected+"'");
            //}
            // Assert
            //Assert.Fail("Need to figure out error message here.");

            //Assert.Equal("Result : " + "ERROR", result);
        }

    }
}
