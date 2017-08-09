using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class CustomTagTests
    {
        [Fact]
        public void It_Should_Parse_A_Custom_Tag()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagRenderer<EchoArgsTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}", templateContext);

            // Assert
            Assert.Equal("Result : I heard string:hello, numeric:123, bool:true", result);

        }

        [Fact]
        public void It_Should_Resolve_A_Variable()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagRenderer<EchoArgsTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("{% assign planet = \"world\"%}Result : {% echoargs \"hello\" planet %}", templateContext);

            // Assert
            Assert.Equal("Result : I heard string:hello, string:world", result);

        }

        [Fact]
        public void It_Should_Render_An_Error_When_No_Tag()
        {
            // Act
            var templateContext = new TemplateContext();

            //var result = RenderingHelper.RenderTemplate("Result : {% awefawef %}", templateContext);
            var template = LiquidTemplate.Create("Result : {% awefawef %}");
            var result = template.LiquidTemplate.Render(templateContext);

            Console.WriteLine(result);
            // Assert
            Assert.Contains("Unknown tag 'awefawef'", result.Result);

        }



        // ReSharper disable once ClassNeverInstantiated.Global
        public class EchoArgsTagRenderer : ICustomTagRenderer
        {
            public IList<String> KeyWords
            {
                get { return new List<string>(); }
            }

            public LiquidString Render(ITemplateContext templateContext, IList<Option<ILiquidValue>> args)
            {
                var argsAsString = String.Join(", ", args.Select(x => x.Value.LiquidTypeName+":"+ValueCaster.RenderAsString(x)));
                return LiquidString.Create("I heard " + argsAsString);
            }
        }

    }
}
