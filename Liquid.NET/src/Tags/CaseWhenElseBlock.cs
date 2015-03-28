using System.Collections.Generic;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CaseWhenElseBlock : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        private readonly IList<WhenBlock> _whenblocks = new List<WhenBlock>();

        public IList<WhenBlock> WhenBlocks { get { return _whenblocks; } }


        public void AddWhenBlock(WhenBlock elseSymbol)
        {
            _whenblocks.Add(elseSymbol);
        }

        public TreeNode<ObjectExpression> ObjectExpressionTree { get; set; }

        public class WhenBlock
        {
            // the blocks to render When This Matches
            public TreeNode<IASTNode> RootContentNode = new TreeNode<IASTNode>(new RootDocumentSymbol());

            // The expression to evaluate
            public TreeNode<ObjectExpression> ObjectExpressionTree { get; set; }

        }
    }
}
