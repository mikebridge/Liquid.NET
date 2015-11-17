using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
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
            LiquidExpressionTrees = new List<TreeNode<IExpressionDescription>>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IList<TreeNode<IExpressionDescription>> LiquidExpressionTrees { get; private set; }

    }
}
