using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class ForRenderer
    {
        private readonly RenderingVisitor _renderingVisitor;

        public ForRenderer(RenderingVisitor renderingVisitor)
        {
            _renderingVisitor = renderingVisitor;
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

                // TODO: the Eval may result in an LiquidCollection with no Array
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

        private void IterateBlock(ForBlockTag forBlockTag, ITemplateContext templateContext, List<ILiquidValue> iterable)
        {
            var offset = LiquidNumeric.Create(0);
            LiquidNumeric limit = null;
            if (!templateContext.Options.NoForLimit)
            {
                limit = LiquidNumeric.Create(50); // see: https://docs.shopify.com/themes/liquid-documentation/tags/iteration-tags#for             
            }
            if (forBlockTag.Offset != null)
            {
                //var result = LiquidExpressionEvaluator.Eval(forBlockTag.Offset, templateContext);
                var result = new LiquidExpressionVisitor(templateContext).Traverse(forBlockTag.Offset).Result;
                if (result.IsSuccess)
                {
                    offset = result.SuccessValue<LiquidNumeric>();
                }
            }
            if (forBlockTag.Limit != null)
            {
                //var result = LiquidExpressionEvaluator.Eval(forBlockTag.Limit, templateContext);
                var result = new LiquidExpressionVisitor(templateContext).Traverse(forBlockTag.Limit).Result;
                if (result.IsSuccess)
                {
                    limit = result.SuccessValue<LiquidNumeric>();
                }
            }

            var subset = iterable.Skip(offset.IntValue);
            if (limit != null)
            {
                subset = subset.Take(limit.IntValue);
            }
            var subsetList = subset.ToList();
            var length = subsetList.Count;
            if (length <= 0) 
            {
                _renderingVisitor.StartWalking(forBlockTag.ElseBlock);
                return;
            }

            int iter = 0;
            //foreach (var item in iterable.Skip(offset.IntValue).Take(limit.IntValue))
            foreach (var item in subsetList)
            {
                String typename = item == null ? "null" : item.LiquidTypeName;
                templateContext.SymbolTableStack.Define("forloop", CreateForLoopDescriptor(
                    forBlockTag.LocalVariable + "-" + typename, iter, length, templateContext.SymbolTableStack));
                templateContext.SymbolTableStack.Define(forBlockTag.LocalVariable, Option<ILiquidValue>.Create(item));

                try
                {
                    _renderingVisitor.StartWalking(forBlockTag.LiquidBlock);
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

        public static LiquidHash CreateForLoopDescriptor(String name, int iter, int length, SymbolTableStack stack)
        {
            return new LiquidHash
            {               
                {"parentloop", FindParentLoop(stack)}, // see: https://github.com/Shopify/liquid/pull/520
                {"first", new LiquidBoolean(iter == 0)},
                {"index", LiquidNumeric.Create(iter + 1)},
                {"index0", LiquidNumeric.Create(iter)},
                {"rindex", LiquidNumeric.Create(length - iter)},
                {"rindex0", LiquidNumeric.Create(length - iter - 1)},
                {"last", new LiquidBoolean(length - iter - 1 == 0)},
                {"length", LiquidNumeric.Create(length) },
                {"name", LiquidString.Create(name) }
            };

        }

        private static Option<ILiquidValue> FindParentLoop(SymbolTableStack stack)
        {
            var parentLoop = stack.Reference("forloop", skiplevels:1 );

            if (parentLoop.IsError || !parentLoop.SuccessResult.HasValue)
            {
                return new None<ILiquidValue>();
            }
            return parentLoop.SuccessValue<LiquidHash>();
                
        }
    }
}
