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
        /// <param name="forBlockTag"></param>
        /// <param name="symbolTableStack"></param>
        public void Render(ForBlockTag forBlockTag, SymbolTableStack symbolTableStack)
        {
            var localBlockScope = new SymbolTable();
            symbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = forBlockTag.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(symbolTableStack).ToList();

                if (forBlockTag.Reversed.BoolValue)
                {
                    iterable.Reverse(); // stupid thing does it in-place.
                }
                IterateBlock(forBlockTag, symbolTableStack, iterable);
            }
            finally
            {
                symbolTableStack.Pop();
            }
        }

        private void IterateBlock(ForBlockTag forBlockTag, SymbolTableStack symbolTableStack, List<IExpressionConstant> iterable)
        {
            var offset = new NumericValue(0);
            var  limit = new NumericValue(50);
            if (forBlockTag.Offset != null)
            {
                var result = LiquidExpressionEvaluator.Eval(forBlockTag.Offset, symbolTableStack);
                if (result.IsSuccess)
                {
                    offset = result.SuccessValue<NumericValue>();
                }
            }
            if (forBlockTag.Limit != null)
            {
                var result = LiquidExpressionEvaluator.Eval(forBlockTag.Limit, symbolTableStack);
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
                symbolTableStack.Define("forloop", CreateForLoopDescriptor(
                    forBlockTag.LocalVariable + "-" + typename, iter, length, symbolTableStack));
                symbolTableStack.Define(forBlockTag.LocalVariable, item);

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
                {"index", new NumericValue(iter + 1 )},
                {"index0", new NumericValue(iter)},
                {"rindex", new NumericValue(length - iter )},
                {"rindex0", new NumericValue(length - iter - 1)},
                {"last", new BooleanValue(length - iter - 1 == 0)},
                {"length", new NumericValue(length) },
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
