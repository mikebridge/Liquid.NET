using System;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public class LiquidASTRenderer
    {

        public string Render(ITemplateContext ctx, LiquidAST liquidAst)
        {
            var renderingVisitor = new RenderingVisitor(this, ctx);
            StartVisiting(renderingVisitor, liquidAst.RootNode);
            if (renderingVisitor.HasErrors)
            {
                throw new LiquidRendererException(renderingVisitor.Errors);
            }
            return renderingVisitor.Text;
        }
        
        public void StartVisiting(IASTVisitor visitor, TreeNode<IASTNode> rootNode)
        {
            rootNode.Data.Accept(visitor);
            rootNode.Children.ForEach(child => StartVisiting(visitor, child));
        }



//        public string EvalTree(ITemplateContext templateContext, LiquidAST liquidAst)
//        {
//           
//        }

    }
}


