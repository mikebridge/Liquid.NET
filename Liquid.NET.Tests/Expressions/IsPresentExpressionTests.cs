using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class IsPresentExpressionTests
    {
        [Test]
        [TestCase("\"\"", "==", false)]
        [TestCase("\"x\"", "==", true)]
        [TestCase("x", "==", false)] 
        [TestCase("0", "==", true)]
        [TestCase("-1", "==", true)]
        [TestCase("\"  \"", "==", false)]
        [TestCase("null", "==", false)]
        [TestCase("null", "!=", true)]
        [TestCase("\"\"", "!=", true)]
        [TestCase("\" \"", "!=", true)]
        [TestCase("0", "!=", false)]
        public void It_Should_Test_That_A_Value_Is_Present(String val, String op, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "PRESENT" : "NOT PRESENT";

            // Act
            var tmpl = @"Result : {% if "+val+" "+op+" present %}PRESENT{% else %}NOT PRESENT{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expectedStr));

                
        }

        [Test]
        [TestCase("\"\"", false)]
        [TestCase("\"x\"",  true)]
        [TestCase("x",  false)]
        [TestCase("0",  true)]
        [TestCase("-1", true)]
        [TestCase("\"  \"",  false)]
        [TestCase("null",  false)]
        public void It_Should_Test_That_Present_With_Question_Mark_Is_Alias(String val, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "PRESENT" : "NOT PRESENT";

            // Act
            var tmpl = @"Result : {% if " + val + ".present? %}PRESENT{% else %}NOT PRESENT{% endif %}";
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log("Value is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expectedStr));


        }

        [Test]
        public void It_Should_Return_True_If_A_Dictionary_Value_Is_Present()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == present %}PRESENT{% else %}NOT PRESENT{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash {
                {"x", new LiquidString("a string")}
            });

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.That(result, Is.EqualTo("Result : PRESENT"));

        }

        [Test]
        public void It_Should_Return_False_If_A_Dictionary_Value_Is_Empty()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == present %}PRESENT{% else %}NOT PRESENT{% endif %}";
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("dict", new LiquidHash());

            // Act
            Logger.Log(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Logger.Log("Value is " + result);
            Assert.That(result, Is.EqualTo("Result : NOT PRESENT"));

        }

        [Test]
        public void It_Should_Not_Accept_Two_Args()
        {
            // Arrange
            var expr = new IsPresentExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false)
            });
            Assert.That(result.IsError);

        }


    }


}
