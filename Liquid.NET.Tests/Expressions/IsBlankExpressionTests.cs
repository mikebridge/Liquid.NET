using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class IsBlankExpressionTests
    {
        [Theory]
        [InlineData("\"\"", "==", true)]
        [InlineData("\" \"", "==", true)]
        [InlineData("\"x\"", "==", false)]
        [InlineData("x", "==", true)]  // nil is blank
        [InlineData("0", "==", false)]
        [InlineData("-1", "==", false)]
        [InlineData("\"  \"", "==", true)]
        [InlineData("null", "==", true)]
        [InlineData("null", "!=", false)]
        [InlineData("\"\"", "!=", false)]
        [InlineData("\" \"", "!=", false)]
        [InlineData("0", "!=", true)]
        public void It_Should_Test_That_A_Value_Is_Blank(String val, String op, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"Result : {% if " + val + " " + op + " blank %}BLANK{% else %}NOT BLANK{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.Equal("Result : " + expectedStr, result);

                
        }

        [Fact]
        public void It_Should_Not_Accept_Two_Args()
        {
            // Arrange
            var expr = new IsBlankExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false)
            });
            Assert.True(result.IsError);

        }

        [Fact]
        public void It_Should_Return_True_With_No_Args()
        {
            // Arrange
            var expr = new IsBlankExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>());
          
            Assert.True(result.SuccessValue<LiquidBoolean>().BoolValue);

        }


        [Theory]
        [InlineData("\"\"", true)]
        [InlineData("\" \"",true)]
        [InlineData("\"x\"",  false)]
        [InlineData("x", true)]  // nil is blank
        [InlineData("0",  false)]
        [InlineData("-1", false)]
        [InlineData("\"  \"", true)]
        [InlineData("null", true)]
        public void It_Should_Test_That_Blank_With_Question_Mark_Is_Alias(String val, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"Result : {% if " + val + ".blank? %}BLANK{% else %}NOT BLANK{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.Equal("Result : " + expectedStr, result);

        }

        [Theory]
        [InlineData("1,2", false)]
        [InlineData("", true)]
        public void It_Should_Test_That_An_Array_Is_Not_Blank(String arr, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"{% assign myarr = '"+arr+@"' | split: '1' %}Result : {% if myarr == blank %}BLANK{% else %}NOT BLANK{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);

            // Assert
            Assert.Equal("Result : " + expectedStr, result);


        }

        [Fact]
        public void It_Should_Return_Flase_If_A_Dictionary_Value_Is_Present()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == blank %}BLANK{% else %}NOT BLANK{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash
            {
                {"x", LiquidString.Create("a string")}
            });

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.Equal("Result : NOT BLANK", result);

        }

        [Fact]
        public void It_Should_Return_True_If_A_Dictionary_Value_Is_Empty()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == BLANK %}BLANK{% else %}NOT BLANK{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash());

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.Equal("Result : NOT BLANK", result);

        }


        [Fact]
        public void It_Should_Return_True_If_LiquidString_Wraps_Null()
        {
            // Arrange
            var str = LiquidString.Create(null);
            var tmpl = @"Result : {% if str == blank %}BLANK{% else %}NOT BLANK{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("str", str);

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.Equal("Result : BLANK", result);

        }

    }


}
