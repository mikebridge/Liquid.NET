using System;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public class LiquidASTRenderer
    {

        public string Render(ITemplateContext ctx, LiquidAST liquidAst)
        {
            String result = "";
            var renderingVisitor = new RenderingVisitor(this, ctx, str => result+=str);
            StartVisiting(renderingVisitor, liquidAst.RootNode);
            if (renderingVisitor.HasErrors)
            {
                throw new LiquidRendererException(renderingVisitor.Errors);
            }
            return result;
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


