using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;

namespace Liquid.NET.src.Rendering
{
    public class TableRowRenderer
    {
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidASTRenderer _astRenderer;

        public TableRowRenderer(RenderingVisitor renderingVisitor, LiquidASTRenderer astRenderer)
        {
            _renderingVisitor = renderingVisitor;
            _astRenderer = astRenderer;
        }
        public void Render(TableRowBlockTag tableRowBlockTag, SymbolTableStack symbolTableStack)
        {
            var localBlockScope = new SymbolTable();
            symbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = tableRowBlockTag.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(symbolTableStack).ToList();

                // TODO: SEE ForRenderer

                // THis is just a placeholder
                foreach (var iter in iterable)
                {

                    _astRenderer.StartVisiting(_renderingVisitor, tableRowBlockTag.LiquidBlock);
                }
                //IterateBlock(forBlockTag, symbolTableStack, iterable);
            }
            finally
            {
                symbolTableStack.Pop();
            }
        }
    }
}
