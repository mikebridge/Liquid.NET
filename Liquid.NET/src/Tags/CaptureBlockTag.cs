using System;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CaptureBlockTag :IASTNode
    {
        public String VarName { get; set; }

        public TreeNode<IASTNode> RootContentNode = new TreeNode<IASTNode>(new RootDocumentNode());

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
