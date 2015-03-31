using System;
using System.Linq;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Array;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public class LiquidEvaluator
    {

        public String Render(ITemplateContext context, LiquidAST liquidAst)
        {
            var symbolTableStack = SymbolTableStackFactory.CreateSymbolTableStack(context);
            return EvalTree(symbolTableStack, liquidAst);
        }

        private string EvalTree(SymbolTableStack symbolStack, LiquidAST liquidAst)
        {
            var renderingVisitor = new RenderingVisitor(this, symbolStack);
            StartVisiting(renderingVisitor, liquidAst.RootNode);
            return renderingVisitor.Text;
        }

        public void StartVisiting(IASTVisitor visitor, TreeNode<IASTNode> rootNode)
        {
            rootNode.Data.Accept(visitor);
            rootNode.Children.ForEach(child => StartVisiting(visitor, child));
        }




    }
}


