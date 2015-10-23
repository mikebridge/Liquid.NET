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
        [TestCase("x")]
        [TestCase("e[1]")]
        [TestCase("e.x")]
        [TestCase("d.x")]
        [TestCase("d[x]")]
        [TestCase("a.b.c")]
        public void It_Should_Display_An_Error_When_Dereferencing_Missing_Value(String varname)
        {
            // Arrange

            TemplateContext ctx = new TemplateContext()                              
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));
            ctx.DefineLocalVariable("d", new DictionaryValue(new Dictionary<String,IExpressionConstant>()));
           
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ "+varname+" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : ERROR: "+varname+" is undefined"));

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

            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("e", new ArrayValue(new List<IExpressionConstant>()));
            ctx.DefineLocalVariable("d", new DictionaryValue(new Dictionary<String, IExpressionConstant>()));

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }



    }
}
