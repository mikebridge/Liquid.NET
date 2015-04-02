using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tags.Custom;

namespace Liquid.NET.Rendering
{
    public class MacroRenderer 
    {
        public StringValue Render(MacroBlockTag macroBlocktag, SymbolTableStack symbolTableStack, IList<IExpressionConstant> args)
        {
            var evaluator = new LiquidASTRenderer();
            var macroScope = new SymbolTable();

            var i = 0;
            foreach (var varName in macroBlocktag.Args.Take(args.Count))
            {
                macroScope.DefineVariable(varName, args[i]);
                i++;
            }
            symbolTableStack.Push(macroScope);

            var subRenderer = new RenderingVisitor(evaluator, symbolTableStack);
            evaluator.StartVisiting(subRenderer, macroBlocktag.LiquidBlock);
            symbolTableStack.Pop();
            return new StringValue(subRenderer.Text);



        }
    }
}
