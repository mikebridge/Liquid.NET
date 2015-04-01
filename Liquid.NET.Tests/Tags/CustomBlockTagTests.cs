using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags.Custom;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CustomBlockTagTests
    {
        [Test]
        public void It_Should_Parse_A_Custom_BlockTag()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<EchoArgsAndBlockTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}echo{% endechoargs %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard StringValue:hello, NumericValue:123, BooleanValue:true"));

        }

        [Test]
        public void It_Should_Not_Parse_A_Custom_BlockTag_With_No_End()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<EchoArgsAndBlockTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}echo{% endsomethingelse %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard StringValue:hello, NumericValue:123, BooleanValue:true"));

        }


        [Test]
        public void It_Should_Parse_A_Custom_BlockTag_With_Nested_Liquid()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<EchoArgsAndBlockTagRenderer>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}{% if true %}TRUE{% endif %}{% endechoargs %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : I heard StringValue:hello, NumericValue:123, BooleanValue:true"));

        }

        public class EchoArgsAndBlockTagRenderer : ICustomBlockTagRenderer
        {
 
            public StringValue Render(SymbolTableStack symbolTableStack, IList<IExpressionConstant> args)
            {
                var argsAsString = String.Join(", ", args.Select(x => x.GetType().Name + ":" + ValueCaster.RenderAsString(x)));
                return new StringValue("I heard " + argsAsString);
            }
        }


    }
}
