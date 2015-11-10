using System;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class SliceFilterTests
    {
        [Test]
        [TestCase("Hello", "0", "H")]
        [TestCase("Hello", "1,3", "ell")]
        [TestCase("Hello", "1", "e")]
        [TestCase("Hello", "-3,2", "ll")] // Shopify example is incorrect
        [TestCase("Hello", "2,-3", "")]
        [TestCase("Hello", "3,1", "l")]
        [TestCase("Hello", "10,1", "")]
        public void It_Should_Slice_A_String(String orig, String slice, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"" + orig + "\" | slice : "+slice+" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

        [Test]
        //[TestCase("0", "[ \"a string\" ]")]
        //[TestCase("1,3", "[ 123, 456, false ]")]
        //[TestCase("-3,2", "[ 123, 456 ]")]
        [TestCase("0", "a string")]
        [TestCase("1,3", "123456.0false")]
        [TestCase("-3,2", "123456.0")]
        
        public void It_Should_Slice_An_Array(String slice, string expected)
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArray());
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ array | slice : " + slice + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));


        }

        [Test]
        public void It_Should_Not_Slice_A_Number()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("num", LiquidNumeric.Create(1));

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ num | slice : 1,2 }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ num | slice : 1,2 }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.That(result.Result, Is.StringContaining("Can't slice a object of type"));

        }

        [Test]
        public void It_Should_Handle_Missing_Start_With_Arrays()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArray());
            
            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ array | slice }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ array | slice }}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert
            Assert.That(result.Result, Is.StringContaining("Please pass a start parameter"));

        }

        [Test]
        public void It_Should_Handle_Missing_Start_With_Strings()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("str", LiquidString.Create("Test"));
            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {{ str | slice }}", ctx);
            var template = LiquidTemplate.Create("Result : {{ str | slice }}");
            var result = template.LiquidTemplate.Render(ctx);
            // Assert
            Assert.That(result.Result, Is.StringContaining("Please pass a start parameter"));

        }

        [Test]
        public void It_Should_Slice_Nil()
        {
            // Arrange
            var ctx = new TemplateContext().WithAllFilters();
            //ctx.DefineLocalVariable("array", CreateArray());
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ novar | slice }}", ctx);

            // Assert
            Assert.That(result, Is.StringContaining("Result : "));

        }

        public LiquidCollection CreateArray()
        {
            return new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };

        }


        
    }
}
