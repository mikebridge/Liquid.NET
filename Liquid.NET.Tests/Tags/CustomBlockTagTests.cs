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
        public void It_Should_Not_Parse_A_Custom_BlockTag_With_No_End()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters().WithCustomTagBlockRenderer<WordReverserBlockTag>("echoargs");
            var result = RenderingHelper.RenderTemplate("Result : {% echoargs \"hello\" 123 true %}echo{% endsomethingelse %}", templateContext);

            // Assert
            //Assert.That(result, Is.EqualTo("Result : ohce"));
            Assert.Fail("This needs to throw an error.");
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

        /// <summary>
        /// Reverse each word
        /// </summary>
        public class WordReverserBlockTag : ICustomBlockTagRenderer
        {

            public StringValue Render(
                    SymbolTableStack symbolTableStack, 
                    TreeNode<IASTNode> liquidBlock,
                    IList<IExpressionConstant> args)
            {
                //var argsAsString = String.Join(", ", args.Select(x => x.GetType().Name + ":" + ValueCaster.RenderAsString(x)));

                var result = EvalLiquidBlock(symbolTableStack, liquidBlock);

                var words = result.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                
                return new StringValue(String.Join(" ", 
                    words.Select(x => new String(x.Reverse().ToArray()))));

            }

            private static String EvalLiquidBlock(SymbolTableStack symbolTableStack, TreeNode<IASTNode> liquidBlock)
            {
                var evaluator = new LiquidASTRenderer();
                var subRenderer = new RenderingVisitor(evaluator, symbolTableStack);
                evaluator.StartVisiting(subRenderer, liquidBlock);
                return subRenderer.Text;
            }
        }


    }
}
