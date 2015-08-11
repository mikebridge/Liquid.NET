using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [TestCase("x", "==", false)]  // nil != empty
        [TestCase("0", "==", false)]
        [TestCase("-1", "==", false)]
        [TestCase("\"  \"", "==", true)]
        [TestCase("null", "==", false)]
        [TestCase("null", "!=", true)]
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
        [TestCase("\"x\"", false)]
        [TestCase("x", false)]  // nil != empty
        [TestCase("0", false)]
        [TestCase("-1", false)]
        [TestCase("\"  \"", true)]
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

    }


}
