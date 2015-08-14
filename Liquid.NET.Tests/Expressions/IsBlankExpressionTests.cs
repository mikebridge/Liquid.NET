using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class IsBlankExpressionTests
    {
        [Test]
        [TestCase("\"\"", "==", true)]
        [TestCase("\" \"", "==", true)]
        [TestCase("\"x\"", "==", false)]
        [TestCase("x", "==", true)]  // nil is blank
        [TestCase("0", "==", false)]
        [TestCase("-1", "==", false)]
        [TestCase("\"  \"", "==", true)]
        [TestCase("null", "==", true)]
        [TestCase("null", "!=", false)]
        [TestCase("\"\"", "!=", false)]
        [TestCase("\" \"", "!=", false)]
        [TestCase("0", "!=", true)]
        public void It_Should_Test_That_A_Value_Is_Blank(String val, String op, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"Result : {% if " + val + " " + op + " blank %}BLANK{% else %}NOT BLANK{% endif %}";
            Console.WriteLine(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Console.WriteLine("Value is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expectedStr));

                
        }

        [Test]
        [TestCase("\"\"", true)]
        [TestCase("\" \"",true)]
        [TestCase("\"x\"",  false)]
        [TestCase("x", true)]  // nil is blank
        [TestCase("0",  false)]
        [TestCase("-1", false)]
        [TestCase("\"  \"", true)]
        [TestCase("null", true)]
        public void It_Should_Test_That_Blank_With_Question_Mark_Is_Alias(String val, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"Result : {% if " + val + ".blank? %}BLANK{% else %}NOT BLANK{% endif %}";
            Console.WriteLine(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Console.WriteLine("Value is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expectedStr));


        }

        [Test]
        [TestCase("1,2", false)]
        [TestCase("", true)]
        public void It_Should_Test_That_An_Array_Is_Not_Blank(String arr, bool expected)
        {
            // Arrange
            var expectedStr = expected ? "BLANK" : "NOT BLANK";

            // Act
            var tmpl = @"{% assign myarr = '"+arr+@"' | split: '1' %}Result : {% if myarr == blank %}BLANK{% else %}NOT BLANK{% endif %}";
            Console.WriteLine(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl);
            Console.WriteLine("Value is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expectedStr));


        }

        [Test]
        public void It_Should_Return_Flase_If_A_Dictionary_Value_Is_Present()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == blank %}BLANK{% else %}NOT BLANK{% endif %}";
            ITemplateContext ctx = new TemplateContext();
            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"x", new StringValue("a string")}
            };

            ctx.DefineLocalVariable("dict", new DictionaryValue(dict));

            // Act
            Console.WriteLine(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Console.WriteLine("Value is " + result);
            Assert.That(result, Is.EqualTo("Result : NOT BLANK"));

        }

        [Test]
        public void It_Should_Return_True_If_A_Dictionary_Value_Is_Empty()
        {
            // Arrange
            var tmpl = @"Result : {% if dict == BLANK %}BLANK{% else %}NOT BLANK{% endif %}";
            ITemplateContext ctx = new TemplateContext();
            var dict = new Dictionary<String, IExpressionConstant>();

            ctx.DefineLocalVariable("dict", new DictionaryValue(dict));

            // Act
            Console.WriteLine(tmpl);
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Console.WriteLine("Value is " + result);
            Assert.That(result, Is.EqualTo("Result : NOT BLANK"));

        }

    }


}
