using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class MissingValueTests
    {
        [Test]
        [TestCase("x", "x")]
        [TestCase("d.x", "x")]
        [TestCase("d[x]", "x")]
        [TestCase("a.b.c", "a")]
        public void It_Should_Display_An_Error_When_Dereferencing_Missing_Value(String varname, String missingVar)
        {
            // Arrange

            ITemplateContext ctx = new TemplateContext()                              
                .ErrorWhenValueMissing();
            //ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));
            ctx.DefineLocalVariable("d", new DictionaryValue());
           
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ "+varname+" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : ERROR: " + missingVar + " is undefined"));

        }

        [Test]
        [TestCase("e[1]")]
        [TestCase("e.first")]
        [TestCase("e.last")]
        public void It_Should_Display_An_Error_When_Dereferencing_Empty_Array(String varname)
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : ERROR: cannot dereference empty array"));

        }

        [Test]
        public void It_Should_Display_Error_When_Dereferencing_Array_With_Non_Int()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ e.x }}", ctx);

            // Assert
            Assert.That(result, Is.StringContaining("invalid index: 'x'"));

        }

        [Test]
        [TestCase("x")]
        [TestCase("e[1]")]
        [TestCase("e.x")]
        [TestCase("d.x")]
        [TestCase("d[x]")]
        [TestCase("a.b.c")]
        public void It_Should_Not_Display_An_Error_When_Dereferencing_Missing_Value(String varname)
        {
            // Arrange
            Console.WriteLine(varname);
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));
            ctx.DefineLocalVariable("d", new DictionaryValue());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }



    }
}
