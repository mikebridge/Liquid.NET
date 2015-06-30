using System;
using System.Collections.Generic;
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
            //Limit = new NumericValue(50); // as per the Shopify docs
            Reversed = new BooleanValue(false);
            //Offset = new NumericValue(0);
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<LiquidExpression> Limit { get; set; }

        public TreeNode<LiquidExpression> Offset { get; set; } // zero-indexed

        public BooleanValue Reversed { get; set; }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        public TreeNode<IASTNode> ElseBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        /// <summary>
        /// The local variable name (the "item" in "for item in ..."
        /// </summary>
        public String LocalVariable { get; set; }

        public IIterableCreator IterableCreator { get; set; }
    }
}
