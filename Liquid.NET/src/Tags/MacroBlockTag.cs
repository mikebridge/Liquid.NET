using System;
using System.Collections.Generic;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class MacroBlockTag : IASTNode
    {
        public String Name { get; private set; }

        public IList<String> Args { get; set; }

        public MacroBlockTag(String name)
        {
            Name = name;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

    }
}
