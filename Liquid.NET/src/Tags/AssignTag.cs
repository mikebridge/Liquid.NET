using System;

using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class AssignTag : IASTNode
    {
        //private readonly IList<TreeNode<LiquidExpression>> _varIndices = new List<TreeNode<LiquidExpression>>();

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public String VarName { get; set; }

        public VariableReferenceTree VarIndices { get; set; }

        public TreeNode<IExpressionDescription> LiquidExpressionTree { get; set; }

    }
}
