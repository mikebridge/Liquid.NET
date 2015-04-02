using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Liquid.NET.Grammar;

namespace Liquid.NET
{
    public class LiquidTemplate
    {

        private readonly LiquidAST _liquidAst;

        public LiquidTemplate(LiquidAST liquidAst)
        {           
            _liquidAst = liquidAst;
        }

        public String Render(ITemplateContext ctx)
        {
            var liquidEvaluator = new LiquidASTRenderer();
            return liquidEvaluator.Render(ctx, _liquidAst);
        }

        public static LiquidTemplate Create(String template)
        {
            var liquidAst = new LiquidASTGenerator().Generate(template);
            return new LiquidTemplate(liquidAst);
        }

    }
}
