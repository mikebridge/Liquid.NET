using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class MacroRenderer 
    {
        public LiquidString Render(
            RenderingVisitor renderingVisitor,
            MacroBlockTag macroBlocktag,
            ITemplateContext templateContext, 
            IList<Option<ILiquidValue>> args)
        {
            var macroScope = new SymbolTable();

            var i = 0;
            foreach (var varName in macroBlocktag.Args.Take(args.Count))
            {
                macroScope.DefineLocalVariable(varName, args[i]);
                i++;
            }
            templateContext.SymbolTableStack.Push(macroScope);

            String hiddenText = "";

            renderingVisitor.PushTextAccumulator(str => hiddenText += str);
            renderingVisitor.StartWalking(macroBlocktag.LiquidBlock);
            renderingVisitor.PopTextAccumulator();

            templateContext.SymbolTableStack.Pop();

            return LiquidString.Create(hiddenText);



        }
    }
}
