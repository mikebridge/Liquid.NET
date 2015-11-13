using System;
using System.Collections.Generic;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class AssignTag : IASTNode
    {
        private readonly IList<TreeNode<LiquidExpression>> _varIndices = new List<TreeNode<LiquidExpression>>();

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public String VarName { get; set; }

        public IList<TreeNode<LiquidExpression>> VarIndices { get { return _varIndices; } }

        public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

    }
}
