using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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


        [Test]
        public void It_Should_Create_A_For_Like_Loop()
        {
            // Act
            var templateContext = new TemplateContext()
                .WithAllFilters()
                .DefineLocalVariable("array", new ArrayValue(new List<IExpressionConstant>{NumericValue.Create(10),NumericValue.Create(11)} ))
                .WithCustomTagBlockRenderer<ForLikeBlockTag>("forcustom");
            var result = RenderingHelper.RenderTemplate("Result : {% forcustom \"item\" array %}{{item}}{% endforcustom %}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : START CUSTOM FOR LOOP1011END CUSTOM FOR LOOP"));

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

                var words = result.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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


        /// <summary>
        /// Reverse each word
        /// </summary>
        public class ForLikeBlockTag : ICustomBlockTagRenderer
        {

            public StringValue Render(
                    ITemplateContext templateContext,
                    TreeNode<IASTNode> liquidBlock,
                    IList<Option<IExpressionConstant>> args)
            {
                // TODO: Should the scope be created in the iterator?
                var localBlockScope = new SymbolTable();
                templateContext.SymbolTableStack.Push(localBlockScope);
                StringValue result = new StringValue("");
                try
                {
                    // normally you would need to verify that arg0 and arg1 exists and are the correct value types.
                    String varname = args[0].Value.ToString();

                    var iterableFactory = new ArrayValueCreator(
                        new TreeNode<LiquidExpression>(
                            new LiquidExpression { Expression = args[1].Value }));

                    var iterable = iterableFactory.Eval(templateContext).ToList();

                    result = IterateBlock(varname, templateContext, iterable, liquidBlock);
                }
                finally
                {
                    templateContext.SymbolTableStack.Pop();
                }
                return new StringValue("START CUSTOM FOR LOOP" + result.StringVal + "END CUSTOM FOR LOOP");

            }

            private StringValue IterateBlock(
                String varname,
                ITemplateContext templateContext,
                List<IExpressionConstant> iterable,
                TreeNode<IASTNode> liquidBlock)
            {
                // TODO: this creates a new renderer, but probably
                // it should reuse the current one...?
                var astRenderer = new LiquidASTRenderer();
                var subRenderer = new RenderingVisitor(astRenderer, templateContext);
                foreach (var item in iterable)
                {
                    templateContext.SymbolTableStack.Define(varname, item);
                    astRenderer.StartVisiting(subRenderer, liquidBlock);
                }
                // you would also process the errors if any in subRenderer
                return new StringValue(subRenderer.Text);
            }

        }
    }
}
