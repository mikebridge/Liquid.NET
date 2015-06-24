using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class IncludeTag : IASTNode
    {

        /// <summary>
        /// This will be the reference to a virtual file
        /// </summary>
        public TreeNode<LiquidExpression> VirtualFileExpression { get; set; }

        public TreeNode<LiquidExpression> WithExpression { get; set; }

        public TreeNode<LiquidExpression> ForExpression { get; set; }

        public readonly IDictionary<String, TreeNode<LiquidExpression>> Definitions =
            new Dictionary<string, TreeNode<LiquidExpression>>();

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}