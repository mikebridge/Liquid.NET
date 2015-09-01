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
    public class CustomBlockTagTests
    {
        [Test]
        public void It_Should_Parse_A_Custom_BlockTag()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<WordReverserBlockTag>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}echo{% endechoargs %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : ohce"));

        }

        [Test]
        // TODO: FIgure out how to make this return a proper error....
        public void It_Should_Not_Parse_A_Custom_BlockTag_With_No_End()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<WordReverserBlockTag>("echoargs");
            try
            {
                var result = RenderingHelper.RenderTemplate(
                    "Result : {% echoargs \"hello\" 123 true %}echo{% endsomethingelse %}", templateContext);
                Assert.Fail("This should have thrown an error.");
            }
            catch (LiquidParserException ex)
            {
                var allErrors = String.Join(",", ex.LiquidErrors.Select(x => x.ToString()));
                Console.WriteLine(allErrors);
                Assert.That(allErrors, Is.StringContaining("rule custom_blocktag failed predicate"));
            }
        }


        [Test]
        public void It_Should_Parse_A_Custom_BlockTag_With_Nested_Liquid()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<WordReverserBlockTag>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}{% if true %}IT IS TRUE{% endif %}{% endechoargs %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : TI SI EURT"));
               
        }

        [Test]
        public void It_Should_Parse_A_Custom_BlockTag_Along_With_A_Custom_Tag() 
        {
            // Act
            var templateContext = new TemplateContext()
                .WithAllFilters()
                .WithCustomTagBlockRenderer<WordReverserBlockTag>("echoargs")
                .WithCustomTagRenderer<CustomTagTests.EchoArgsTagRenderer>("echoargs2");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs2 \"Test\" %}{% echoargs \"hello\" 123 true %}{% if true %}IT IS TRUE{% endif %}{% endechoargs %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : TI SI EURT"));

        }

        /// <summary>
        /// Reverse each word
        /// </summary>
        public class WordReverserBlockTag : ICustomBlockTagRenderer
        {

            public StringValue Render(
                    ITemplateContext templatecontext, 
                    TreeNode<IASTNode> liquidBlock,
                    IList<Option<IExpressionConstant>> args)
            {
                //var argsAsString = String.Join(", ", args.Select(x => x.GetType().Name + ":" + ValueCaster.RenderAsString(x)));

                var result = EvalLiquidBlock(templatecontext, liquidBlock);

                var words = result.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                
                return new StringValue(String.Join(" ", 
                    words.Select(x => new String(x.Reverse().ToArray()))));

            }

            private static String EvalLiquidBlock(ITemplateContext templateContext, TreeNode<IASTNode> liquidBlock)
            {
                var evaluator = new LiquidASTRenderer();
                var subRenderer = new RenderingVisitor(evaluator, templateContext);
                evaluator.StartVisiting(subRenderer, liquidBlock);
                return subRenderer.Text;
            }

        }


    }
}
