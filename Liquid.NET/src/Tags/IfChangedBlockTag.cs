using System;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    // SEE: http://www.shopify.ca/technology/3026792-new-liquid-tag-ifchanged
    public class IfChangedBlockTag : IASTNode
    {
        public IfChangedBlockTag()
        {
            UniqueId = Guid.NewGuid().ToString();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        public String UniqueId { get; private set; }
    }
}
