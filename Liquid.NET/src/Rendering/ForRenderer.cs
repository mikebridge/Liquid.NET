using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;

namespace Liquid.NET.Rendering
{
    public class ForRenderer
    {
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidEvaluator _evaluator;

        public ForRenderer(RenderingVisitor renderingVisitor, LiquidEvaluator evaluator)
        {
            _renderingVisitor = renderingVisitor;
            _evaluator = evaluator;
        }

        /// <summary>
        /// Side effect; this calls the renderer with the evaluator to render
        /// the block.  It would probably be better to handle this a little more
        /// cleanly.
        /// </summary>
        /// <param name="forBlock"></param>
        /// <param name="symbolTableStack"></param>
        public void Render(ForBlock forBlock, SymbolTableStack symbolTableStack)
        {
            var localBlockScope = new SymbolTable(); // TODO: Put the local "forloop" variables in it.
            symbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = forBlock.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(symbolTableStack).ToList();

                Console.WriteLine("REVERSED: " + forBlock.Reversed.BoolValue);
                if (forBlock.Reversed.BoolValue)
                {
                    iterable.Reverse(); // stupid thing does it in-place.
                }
                var offset = forBlock.Offset.IntValue; // zero indexed
                var limit = forBlock.Limit.IntValue; // max number of iterations.

                // I think the offset and limit just slices the
                // iterable before it goes into the loop.

                int iter = 0;
                int length = iterable.Skip(offset).Take(limit).Count();
                if (length < 0)
                {
                    return;
                }
                foreach (var item in iterable.Skip(offset).Take(limit))
                {
                    symbolTableStack.Define("forloop", CreateForLoopDescriptor(iter, length));

                    symbolTableStack.Define(forBlock.LocalVariable, item);
                    // TODO: This could be handled a little cleaner.
                    _evaluator.StartVisiting(_renderingVisitor, forBlock.RootContentNode);
                    iter ++;
                }
            }
            finally
            {
                symbolTableStack.Pop();
            }
        }

        public static DictionaryValue CreateForLoopDescriptor(int iter, int length)
        {
            return new DictionaryValue(new Dictionary<String, IExpressionConstant>
            {
                {"first", new BooleanValue(iter == 0)},
                {"index", new NumericValue(iter + 1 )},
                {"index0", new NumericValue(iter)},
                {"rindex", new NumericValue(length - iter )},
                {"rindex0", new NumericValue(length - iter - 1)},
                {"last", new BooleanValue(length - iter - 1 == 0)},
                {"length", new NumericValue(length) }
            });

        }
    }
}
