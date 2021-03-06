﻿using System;
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

        public TableRowRenderer(RenderingVisitor renderingVisitor)
        {
            _renderingVisitor = renderingVisitor;
        }

        public void Render(TableRowBlockTag tableRowBlockTag, ITemplateContext templateContext, Action<String> appender)
        {
            var offset = LiquidNumeric.Create(0);
            var  limit = LiquidNumeric.Create(50);
            var cols = LiquidNumeric.Create(5); // TODO: What is the default? https://github.com/Shopify/liquid/blob/master/lib/liquid/tags/table_row.rb
            if (tableRowBlockTag.Offset != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Offset, templateContext);
                if (result.IsSuccess)
                {
                    offset = result.SuccessValue<LiquidNumeric>();
                }
            }
            if (tableRowBlockTag.Limit != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Limit, templateContext);
                if (result.IsSuccess)
                {
                    limit = result.SuccessValue<LiquidNumeric>();
                }
            }
            if (tableRowBlockTag.Cols != null)
            {
                var result = LiquidExpressionEvaluator.Eval(tableRowBlockTag.Cols, templateContext);
                if (result.IsSuccess)
                {
                    cols = result.SuccessValue<LiquidNumeric>();
                }
            }
            var localBlockScope = new SymbolTable();
            templateContext.SymbolTableStack.Push(localBlockScope);
            try
            {
                var iterableFactory = tableRowBlockTag.IterableCreator;

                // TODO: the Eval may result in an LiquidCollection with no Array
                // (i.e. it's undefined).  THe ToList therefore fails....
                // this should use Bind
                var iterable = iterableFactory.Eval(templateContext).ToList();

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
                    templateContext.DefineLocalVariable(tableRowBlockTag.LocalVariable, Option<ILiquidValue>.Create(item));
                    String typename = item == null ? "null" : item.LiquidTypeName;
                    templateContext.DefineLocalVariable("tablerowloop", CreateForLoopDescriptor(
                        tableRowBlockTag.LocalVariable + "-" + typename, 
                        // ReSharper disable RedundantArgumentName
                        iter: iter, 
                        length: length, 
                        col: currentcol, 
                        maxcol: cols.IntValue, 
                        row: currentrow,
                        stack: templateContext.SymbolTableStack));
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
                    _renderingVisitor.StartWalking(tableRowBlockTag.LiquidBlock);
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
                templateContext.SymbolTableStack.Pop();
            }
        }

        public static LiquidHash CreateForLoopDescriptor(String name, int iter, int length, int col, int maxcol, int row, SymbolTableStack stack)
        {
            return new LiquidHash
            {               
                {"first", new LiquidBoolean(iter == 0)},
                {"index", LiquidNumeric.Create(iter + 1)},
                {"col" , LiquidNumeric.Create(col + 1)},
                {"row" , LiquidNumeric.Create(row + 1)},
                {"row0" , LiquidNumeric.Create(row)},
                {"col0" , LiquidNumeric.Create(col)},
                {"col_first" , new LiquidBoolean(col == 0)},
                {"col_last" , new LiquidBoolean(col == maxcol)},
                {"index0", LiquidNumeric.Create(iter)},
                {"rindex", LiquidNumeric.Create(length - iter)},
                {"rindex0", LiquidNumeric.Create(length - iter - 1)},
                {"last", new LiquidBoolean(length - iter - 1 == 0)},
                {"length", LiquidNumeric.Create(length) },
                {"name", LiquidString.Create(name) }
            };

        }

    }
}
