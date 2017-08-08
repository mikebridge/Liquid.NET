using System;
using System.Linq;
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
            ctx.DefineLocalVariable("d", new LiquidHash());
           
            // Act
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result.Result, Is.EqualTo("Result : ERROR: " + missingVar + " is undefined"));

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
            ctx.DefineLocalVariable("e", new LiquidCollection());

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ " + varname + " }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.That(result.Result, Is.EqualTo("Result : ERROR: cannot dereference empty array"));

        }



        [Test]
        public void It_Should_Display_Error_When_Dereferencing_Array_With_Non_Int()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", new LiquidCollection());

            // Act
            var template = LiquidTemplate.Create("Result : {{ e.x }}");
            var result = template.LiquidTemplate.Render(ctx);
            //var result = RenderingHelper.RenderTemplate("Result : {{ e.x }}", ctx);

            // Assert
            Assert.That(result.Result, Does.Contain("invalid index: 'x'"));

        }

        [Test]
        public void It_Should_Display_Error_When_Dereferencing_Primitive_With_Index()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .ErrorWhenValueMissing();
            ctx.DefineLocalVariable("e", LiquidString.Create("Hello"));

            // Act
            var template = LiquidTemplate.Create("Result : {{ e.x }}");
            var result = template.LiquidTemplate.Render(ctx);

            Assert.That(result.HasRenderingErrors, Is.True);
            var errorMessage = String.Join(",", result.RenderingErrors.Select(x => x.Message));
            // Assert
            Assert.That(errorMessage, Does.Contain("invalid string index: 'x'"));

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
            ctx.DefineLocalVariable("e", new LiquidCollection());
            ctx.DefineLocalVariable("d", new LiquidHash());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + varname + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }



    }
}
