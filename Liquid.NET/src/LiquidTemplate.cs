using System;

namespace Liquid.NET
{
    public class LiquidTemplate
    {
        
        private readonly LiquidAST _liquidAst;

        public static LiquidTemplate Create(String template)
        {
            var liquidAst = new LiquidASTGenerator().Generate(template);
            return new LiquidTemplate(liquidAst);
        }

        public LiquidTemplate(LiquidAST liquidAst)
        {           
            _liquidAst = liquidAst;
        }

        public String Render(ITemplateContext ctx, Action<LiquidError> onRenderingError = null)
        {
            onRenderingError = onRenderingError ?? (err => { });

            var result = "";

            var renderingVisitor = new RenderingVisitor(ctx);

            renderingVisitor.RenderingErrorEventHandler += (sender, err) => onRenderingError(err);

            renderingVisitor.StartWalking(
                _liquidAst.RootNode, 
                str => result += str);

            return result;
        }
    }
}
