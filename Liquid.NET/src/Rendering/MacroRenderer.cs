using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class MacroRenderer 
    {
        public StringValue Render(
            MacroBlockTag macroBlocktag,
            ITemplateContext templateContext, 
            IList<Option<IExpressionConstant>> args, 
            IList<LiquidError> errorAccumulator )
        {
            var evaluator = new LiquidASTRenderer();
            var macroScope = new SymbolTable();

            var i = 0;
            foreach (var varName in macroBlocktag.Args.Take(args.Count))
            {
                macroScope.DefineVariable(varName, args[i].HasValue? args[i].Value : null);
                i++;
            }
            templateContext.SymbolTableStack.Push(macroScope);

            var subRenderer = new RenderingVisitor(evaluator, templateContext);
            //if (subRenderer )

            evaluator.StartVisiting(subRenderer, macroBlocktag.LiquidBlock);
            templateContext.SymbolTableStack.Pop();
            foreach (var error in subRenderer.Errors)
            {
                errorAccumulator.Add(error);
            }
            return new StringValue(subRenderer.Text);



        }
    }
}
