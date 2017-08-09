using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Helpers;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class DebugFilterTests
    {
        [Fact]
        public void It_Should_Display_Debugging_Info_For_A_Variable()
        {
            // Arrange
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 3, 30, 23, 1, 12);
            var dateValue = new LiquidDate(dateTime);
            dateValue.MetaData["hello"] = "test";
            ctx.DefineLocalVariable("mydate", dateValue);
            
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | debug }}", ctx);

            // Assert
            Assert.Contains("\"metadata\" : { \"hello\" : \"test\" }", result);
            Assert.Contains("\"value\" : \"03/30/2015 23:01:12\"", result);
            Assert.Contains("\"type\" : \"date", result);
            Assert.Equal("Result : { \"metadata\" : { \"hello\" : \"test\" }, \"value\" : \"03/30/2015 23:01:12\", \"type\" : \"date\" }", result);

        }

        [Fact]
        public void It_Should_Include_The_Included_FileName_WhenAssigned()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Result {% assign x = 123 %}{{ x | debug }}" } });

            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);
            Logger.Log(result);

            // Assert
            Assert.Contains("{ \"assigned\" : \"test\"", result);

        }

        [Fact]
        public void It_Should_Include_The_Included_FileName_When_Reassigned()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "{% assign x = 123 %}" },
                { "test2", "{% assign x = 456 %}" }
            });

            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' %}{% include 'test2' %}{{ x | debug }}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);
            Logger.Log(result);

            // Assert
            //Assert.That(result, Does.Contain("{ \"assigned\" : \"test\""));
            Assert.Contains("{ \"reassigned\" : \"test2\"", result);

        }

        private static ITemplateContext CreateContext(Dictionary<String, String> dict)
        {
            return new TemplateContext().WithFileSystem(new TestFileSystem(dict));
        }


    }
}
