using System;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class AssignTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public String VarName { get; set; }

        public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

    }
}
