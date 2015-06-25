using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
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
            Assert.That(result, Is.EqualTo("Result : I heard StringValue:hello, NumericValue:123, BooleanValue:true"));

        }

        [Test]
        public void It_Should_Resolve_A_Variable()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagRenderer<EchoArgsTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("{% assign planet = \"world\"%}Result : {% echoargs \"hello\" planet %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard StringValue:hello, StringValue:world"));

        }


        public class EchoArgsTagRenderer : ICustomTagRenderer
        {
            public IList<String> KeyWords
            {
                get { return new List<string>(); }
            }

            public StringValue Render(SymbolTableStack symbolTableStack, IList<Option<IExpressionConstant>> args)
            {
                var argsAsString = String.Join(", ", args.Select(x => x.GetType().Name+":"+ValueCaster.RenderAsString(x)));
                return new StringValue("I heard " + argsAsString);
            }
        }

    }
}
