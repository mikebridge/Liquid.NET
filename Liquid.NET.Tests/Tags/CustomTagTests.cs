using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags.Custom;
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
            Assert.That(result, Is.EqualTo("I heard \"hello\" 123 true"));

        }

        public class EchoArgsTagRenderer : ICustomTagRenderer
        {
            public IList<String> KeyWords
            {
                get { return new List<string>(); }
            }

            public StringValue Render(SymbolTableStack symbolTableStack, IList<IExpressionConstant> args)
            {
                var argsAsString = String.Join(", ", args.Select(ValueCaster.RenderAsString));
                return new StringValue("Result: "+argsAsString);
            }
        }

    }
}
