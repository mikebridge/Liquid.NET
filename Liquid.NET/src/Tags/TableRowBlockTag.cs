using System;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class TableRowBlockTag : IASTNode
    {
        public TreeNode<IExpressionDescription> Cols;
        public TreeNode<IExpressionDescription> Limit;
        public TreeNode<IExpressionDescription> Offset;
        //public TreeNode<LiquidExpression> Range;

        public void Accept(IASTVisitor visitor)
        {
           visitor.Visit(this);
        }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        /// <summary>
        /// The local variable name (the "item" in "tablerow item in ..."
        /// </summary>
        public String LocalVariable { get; set; }

        public IIterableCreator IterableCreator { get; set; }

    }
}
