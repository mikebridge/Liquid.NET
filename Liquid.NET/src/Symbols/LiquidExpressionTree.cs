using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class LiquidExpressionTree : IASTNode
    {

        public LiquidExpressionTree(TreeNode<IExpressionDescription> liquidExpressionTree)
        {
            ExpressionTree = liquidExpressionTree;
        }

        public TreeNode<IExpressionDescription> ExpressionTree { get; private set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
