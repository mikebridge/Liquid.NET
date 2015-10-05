using System;
using System.Collections.Generic;
using Liquid.NET.Grammar;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CustomTag : IASTNode
    {
        public String TagName { get; private set; }

        public CustomTag(String tagName)
        {
            TagName = tagName;
            LiquidExpressionTrees = new List<TreeNode<LiquidExpression>>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IList<TreeNode<LiquidExpression>> LiquidExpressionTrees { get; private set; }

        public String RawText { get; set; }
    }
}
