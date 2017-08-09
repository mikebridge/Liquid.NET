using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class IsEmptyExpressionTests
    {
        [Theory]
        [InlineData("\"\"", "==", true)]
        [InlineData("\" \"", "==", false)]
        [InlineData("\"x\"", "==", false)]
        [InlineData("x", "==", true)]  // nil == empty
        [InlineData("0", "==", false)]
        [InlineData("-1", "==", false)]
        [InlineData("\"  \"", "==", false)]
        [InlineData("null", "==", true)]
        [InlineData("null", "!=", false)]
        [InlineData("\"\"", "!=", false)]
        [InlineData("\" \"", "!=", true)]
        [InlineData("0", "!=", true)]
        public void It_Should_Test_That_A_Value_Is_Empty(String val, String op, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "EMPTY" : "NOT EMPTY";

            // Act
            var tmpl = @"Result : {% if "+val+" "+op+" empty %}EMPTY{% else %}NOT EMPTY{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.Equal("Result : " + expectedStr, result);

                
        }

        [Theory]
        [InlineData("\"\"", true)]
        [InlineData("\" \"",  false)]
        [InlineData("\"x\"", false)]
        [InlineData("x", true)]  // nil == empty
        [InlineData("0", false)]
        [InlineData("-1",  false)]
        [InlineData("\"  \"",  false)]
        [InlineData("null", true)]
        public void It_Should_Test_That_Empty_WIth_Question_Mark_Is_Alias(String val, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "EMPTY" : "NOT EMPTY";

            // Act
            var tmpl = @"Result : {% if " + val + ".empty? %}EMPTY{% else %}NOT EMPTY{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.Equal("Result : " + expectedStr, result);

        }

        [Fact]
        public void It_Should_Return_False_If_A_Dictionary_Value_Is_Present()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == empty %}EMPTY{% else %}NOT EMPTY{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash{
                {"x", LiquidString.Create("a string")}
            });

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.Equal("Result : NOT EMPTY", result);

        }

        [Fact]
        public void It_Should_Return_True_If_A_Dictionary_Value_Is_Empty()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == empty %}EMPTY{% else %}NOT EMPTY{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash());

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.Equal("Result : EMPTY", result);

        }

        [Fact]
        public void It_Should_Not_Accept_Two_Args()
        {
            // Arrange
            var expr = new IsEmptyExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false)
            });
            Assert.True(result.IsError);

        }

    }


}
