using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class ForBlock : IASTNode
    {

        public ForBlock()
        {
            this.Limit = new NumericValue(50); // as per the Shopify docs
            this.Reversed = new BooleanValue(false);
            this.Offset = new NumericValue(0);
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// TODO: the documentation here is confusing.
        /// https://docs.shopify.com/themes/liquid-documentation/tags/iteration-tags
        /// </summary>
        public NumericValue Limit { get; set; }

        public NumericValue Offset { get; set; } // zero-indexed

        public BooleanValue Reversed { get; set; }

        public TreeNode<IASTNode> RootContentNode = new TreeNode<IASTNode>(new RootDocumentSymbol());

        public String LocalVariable { get; set; }

        public IIterableCreator IterableCreator { get; set; }
    }
}
