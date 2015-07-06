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
        private readonly LiquidASTRenderer _liquidEvaluator;

        public LiquidTemplate(LiquidAST liquidAst)
        {           
            _liquidAst = liquidAst;
            _liquidEvaluator = new LiquidASTRenderer();
        }

        public String Render(ITemplateContext ctx)
        {
            return _liquidEvaluator.Render(ctx, _liquidAst);
        }

        public static LiquidTemplate Create(String template)
        {
            var liquidAst = new LiquidASTGenerator().Generate(template);
            return new LiquidTemplate(liquidAst);
        }

    }
}
