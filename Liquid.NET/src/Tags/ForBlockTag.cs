using System;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class ForBlockTag : IASTNode
    {

        public ForBlockTag()
        {
            //Limit = new LiquidNumeric(50); // as per the Shopify docs
            Reversed = new LiquidBoolean(false);
            //Offset = new LiquidNumeric(0);
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<IExpressionDescription> Limit { get; set; }

        public TreeNode<IExpressionDescription> Offset { get; set; } // zero-indexed

        public LiquidBoolean Reversed { get; set; }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        public TreeNode<IASTNode> ElseBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        /// <summary>
        /// The local variable name (the "item" in "for item in ..."
        /// </summary>
        public String LocalVariable { get; set; }

        public IIterableCreator IterableCreator { get; set; }
    }
}
