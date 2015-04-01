using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CustomBlockTag : IASTNode
    {

        public String TagName { get; private set; }

        public CustomBlockTag(String tagName)
        {
            TagName = tagName;
            LiquidExpressionTrees = new List<TreeNode<LiquidExpression>>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IList<TreeNode<LiquidExpression>> LiquidExpressionTrees { get; private set; }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

    }
}
