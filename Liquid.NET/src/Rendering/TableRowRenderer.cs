using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
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
            var localBlockScope = new SymbolTable();
            symbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = tableRowBlockTag.IterableCreator;

                // TODO: the Eval may result in an ArrayValue with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(symbolTableStack).ToList();

                int length = iterable.Skip(offset.IntValue).Take(limit.IntValue).Count();

                if (length == 0)
                {
                    appender(" <tr class=\"row1\">\r\n</tr>"); // this is what ruby liquid does.
                    return;
                }
                int currentrow = 0;
                int currentcol = 0;

                bool needsEndOfRow = false;
                int iter = 0;
                foreach (var item in iterable.Skip(offset.IntValue).Take(limit.IntValue))
                {
                    symbolTableStack.Define(tableRowBlockTag.LocalVariable, item);
                    String typename = item == null ? "null" : item.LiquidTypeName;
                    symbolTableStack.Define("tablerowloop", CreateForLoopDescriptor(
                        tableRowBlockTag.LocalVariable + "-" + typename, 
                        // ReSharper disable RedundantArgumentName
                        iter: iter, 
                        length: length, 
                        col: currentcol, 
                        maxcol: cols.IntValue, 
                        row: currentrow, 
                        stack: symbolTableStack));
                        // ReSharper restore RedundantArgumentName
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
                    iter++;
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

        public static DictionaryValue CreateForLoopDescriptor(String name, int iter, int length, int col, int maxcol, int row, SymbolTableStack stack)
        {
            return new DictionaryValue(new Dictionary<String, Option<IExpressionConstant>>
            {               
                {"first", new BooleanValue(iter == 0)},
                {"index", new NumericValue(iter + 1 )},
                {"col" , new NumericValue(col + 1)},
                {"row" , new NumericValue(row + 1)},
                {"row0" , new NumericValue(row)},
                {"col0" , new NumericValue(col)},
                {"col_first" , new BooleanValue(col == 0)},
                {"col_last" , new BooleanValue(col == maxcol)},
                {"index0", new NumericValue(iter)},
                {"rindex", new NumericValue(length - iter )},
                {"rindex0", new NumericValue(length - iter - 1)},
                {"last", new BooleanValue(length - iter - 1 == 0)},
                {"length", new NumericValue(length) },
                {"name", new StringValue(name) }
            });

        }

    }
}
