using System;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CommentBlock : IASTNode
    {
        private readonly string _rawText;

        public CommentBlock(String rawText)
        {
            _rawText = rawText;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<IASTNode> RootNode = new TreeNode<IASTNode>(new RootDocumentSymbol()); 
    }
}
