using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CommentBlockTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<IASTNode> RootNode = new TreeNode<IASTNode>(new RootDocumentNode()); 
    }
}
