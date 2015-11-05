using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class LiquidExpressionTree : IASTNode
    {

        public LiquidExpressionTree(TreeNode<LiquidExpression> liquidExpressionTree)
        {
            ExpressionTree = liquidExpressionTree;
        }

        public TreeNode<LiquidExpression> ExpressionTree { get; private set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
