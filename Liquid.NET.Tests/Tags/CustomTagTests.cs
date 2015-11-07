using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CustomTagTests
    {
        [Test]
        public void It_Should_Parse_A_Custom_Tag()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagRenderer<EchoArgsTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard string:hello, numeric:123, bool:true"));

        }

        [Test]
        public void It_Should_Resolve_A_Variable()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagRenderer<EchoArgsTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("{% assign planet = \"world\"%}Result : {% echoargs \"hello\" planet %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard string:hello, string:world"));

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
