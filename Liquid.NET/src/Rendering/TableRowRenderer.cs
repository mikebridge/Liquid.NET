using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
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
        public void Render(TableRowBlockTag tableRowBlockTag, SymbolTableStack symbolTableStack, Func<String, String> appender)
        {
            var offset = new NumericValue(0);
            var  limit = new NumericValue(50);
            var cols = new NumericValue(5); // TODO: What is the default? https://github.com/Shopify/liquid/blob/master/lib/liquid/tags/table_row.rb
            if (tableRowBlockTag.Offset != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Offset, symbolTableStack);
                if (result.IsSuccess)
                {
                    offset = result.SuccessValue<NumericValue>();
                }
            }
            if (tableRowBlockTag.Limit != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Limit, symbolTableStack);
                if (result.IsSuccess)
                {
                    limit = result.SuccessValue<NumericValue>();
                }
            }
            if (tableRowBlockTag.Cols != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Cols, symbolTableStack);
                if (result.IsSuccess)
                {
                    cols = result.SuccessValue<NumericValue>();
                }
            }
            Console.WriteLine("OFFSET IS " + offset.IntValue);
            Console.WriteLine("LIMIT IS " + limit.IntValue);
            Console.WriteLine("COLS IS " + cols.IntValue);
            var localBlockScope = new SymbolTable();
            symbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = tableRowBlockTag.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(symbolTableStack).ToList();
                if (iterable.Count == 0)
                {
                    return; // don't render an empty row.
                }
                // TODO: SEE ForRenderer

                // THis is just a placeholder
                int currentrow = 0;
                int currentcol = 0;

                bool needsEndOfRow = false;
                foreach (var item in iterable.Skip(offset.IntValue).Take(limit.IntValue))
                {
                    symbolTableStack.Define(tableRowBlockTag.LocalVariable, item);
                    int rowFromOne = currentrow + 1;
                    int colFromOne = currentcol + 1;

                    if (currentcol == 0)
                    {
                        needsEndOfRow = true;
                        appender(@"<tr class=""row" + rowFromOne + "\">");
                        if (currentrow == 0) // ruby liquid prints end-of-line on first row.
                        {
                            appender("\r\n");
                        }
                    }
                    appender("<td class=\"col" + colFromOne + @""">");
                    _astRenderer.StartVisiting(_renderingVisitor, tableRowBlockTag.LiquidBlock);
                    appender(@"</td>");
                    currentcol++;

                    if (currentcol >= cols.IntValue)
                    {
                        needsEndOfRow = false;
                        appender("</tr>\r\n");

                        currentcol = 0;
                        currentrow++;
                    }
                }
                if (needsEndOfRow)
                {
                    appender("</tr>\r\n");
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
