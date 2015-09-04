using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class ForRenderer
    {
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidASTRenderer _astRenderer;

        public ForRenderer(RenderingVisitor renderingVisitor, LiquidASTRenderer astRenderer)
        {
            _renderingVisitor = renderingVisitor;
            _astRenderer = astRenderer;
        }

        /// <summary>
        /// this calls the renderer with the astRenderer to render
        /// the block.
        /// </summary>
        public void Render(ForBlockTag forBlockTag, ITemplateContext templateContext)
        {
            var localBlockScope = new SymbolTable();
            templateContext.SymbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = forBlockTag.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(templateContext).ToList();

                if (forBlockTag.Reversed.BoolValue)
                {
                    iterable.Reverse(); // stupid thing does it in-place.
                }
                IterateBlock(forBlockTag, templateContext, iterable);
            }
            finally
            {
                templateContext.SymbolTableStack.Pop();
            }
        }

        private void IterateBlock(ForBlockTag forBlockTag, ITemplateContext templateContext, List<IExpressionConstant> iterable)
        {
            var offset = NumericValue.Create(0);
            var  limit = NumericValue.Create(50);
            if (forBlockTag.Offset != null)
            {
                var result = LiquidExpressionEvaluator.Eval(forBlockTag.Offset, templateContext);
                if (result.IsSuccess)
                {
                    offset = result.SuccessValue<NumericValue>();
                }
            }
            if (forBlockTag.Limit != null)
            {
                var result = LiquidExpressionEvaluator.Eval(forBlockTag.Limit, templateContext);
                if (result.IsSuccess)
                {
                    limit = result.SuccessValue<NumericValue>();
                }
            }

            int length = iterable.Skip(offset.IntValue).Take(limit.IntValue).Count();
            if (length <= 0) 
            {
                _astRenderer.StartVisiting(_renderingVisitor, forBlockTag.ElseBlock);
                return;
            }

            int iter = 0;
            foreach (var item in iterable.Skip(offset.IntValue).Take(limit.IntValue))
            {
                String typename = item == null ? "null" : item.LiquidTypeName;
                templateContext.SymbolTableStack.Define("forloop", CreateForLoopDescriptor(
                    forBlockTag.LocalVariable + "-" + typename, iter, length, templateContext.SymbolTableStack));
                templateContext.SymbolTableStack.Define(forBlockTag.LocalVariable, item);

                try
                {
                    _astRenderer.StartVisiting(_renderingVisitor, forBlockTag.LiquidBlock);
                }
                catch (ContinueException)
                {
                    continue;
                }
                catch (BreakException)
                {
                    break;
                }
                iter ++;
            }
        }

        public static DictionaryValue CreateForLoopDescriptor(String name, int iter, int length, SymbolTableStack stack)
        {
            return new DictionaryValue(new Dictionary<String, Option<IExpressionConstant>>
            {               
                {"parentloop", FindParentLoop(stack)}, // see: https://github.com/Shopify/liquid/pull/520
                {"first", new BooleanValue(iter == 0)},
                {"index", NumericValue.Create(iter + 1)},
                {"index0", NumericValue.Create(iter)},
                {"rindex", NumericValue.Create(length - iter)},
                {"rindex0", NumericValue.Create(length - iter - 1)},
                {"last", new BooleanValue(length - iter - 1 == 0)},
                {"length", NumericValue.Create(length) },
                {"name", new StringValue(name) }
            });

        }

        private static Option<IExpressionConstant> FindParentLoop(SymbolTableStack stack)
        {
            var parentLoop = stack.Reference("forloop", skiplevels:1 );

            if (parentLoop.IsError || !parentLoop.SuccessResult.HasValue)
            {
                return new None<IExpressionConstant>();
            }
            return parentLoop.SuccessValue<DictionaryValue>();
                
        }
    }
}
