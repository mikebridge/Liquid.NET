using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class IncludeTag : IASTNode
    {

        /// <summary>
        /// This will be the reference to a virtual file
        /// </summary>
        public TreeNode<IExpressionDescription> VirtualFileExpression { get; set; }

        public TreeNode<IExpressionDescription> WithExpression { get; set; }

        public TreeNode<IExpressionDescription> ForExpression { get; set; }

        public readonly IDictionary<String, TreeNode<IExpressionDescription>> Definitions =
            new Dictionary<string, TreeNode<IExpressionDescription>>();

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}