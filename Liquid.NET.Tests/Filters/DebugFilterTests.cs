using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Tests.Helpers;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class DebugFilterTests
    {
        [Test]
        public void It_Should_Display_Debugging_Info_For_A_Variable()
        {            
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 3, 30, 23, 1, 12);
            var dateValue = new DateValue(dateTime);
            dateValue.MetaData["hello"] = "test";
            ctx.DefineLocalVariable("mydate", dateValue);
            
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | debug }}", ctx);

            // Assert
            Assert.That(result, Is.StringContaining("\"metadata\" : { \"hello\" : \"test\" }"));
            Assert.That(result, Is.StringContaining("\"value\" : \"2015-03-30 11:01:12 pm\""));
            Assert.That(result, Is.StringContaining("\"type\" : \"date"));
            Assert.That(result, Is.EqualTo("Result : { \"metadata\" : { \"hello\" : \"test\" }, \"value\" : \"2015-03-30 11:01:12 pm\", \"type\" : \"date\" }"));

        }

        [Test]
        public void It_Should_Include_The_Included_FileName_WhenAssigned()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Result {% assign x = 123 %}{{ x | debug }}" } });

            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.StringContaining("{ \"assigned\" : \"test\""));

        }

        [Test]
        public void It_Should_Include_The_Included_FileName_When_Reassigned()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "{% assign x = 123 %}" },
                { "test2", "{% assign x = 456 %}" }
            });

            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' %}{% include 'test2' %}{{ x | debug }}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);
            Console.WriteLine(result);

            // Assert
            //Assert.That(result, Is.StringContaining("{ \"assigned\" : \"test\""));
            Assert.That(result, Is.StringContaining("{ \"reassigned\" : \"test2\""));

        }

        private static ITemplateContext CreateContext(Dictionary<String, String> dict)
        {
            return new TemplateContext().WithFileSystem(new TestFileSystem(dict));
        }


    }
}
