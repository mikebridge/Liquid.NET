using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CaptureBlock :IASTNode
    {
        public String VarName { get; set; }

        public TreeNode<IASTNode> RootContentNode = new TreeNode<IASTNode>(new RootDocumentSymbol());

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
