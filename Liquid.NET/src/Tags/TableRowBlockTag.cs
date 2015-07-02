using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class TableRowBlockTag : IASTNode
    {
        public TreeNode<LiquidExpression> Cols;
        public TreeNode<LiquidExpression> Limit;
        public TreeNode<LiquidExpression> Offset;
        //public TreeNode<LiquidExpression> Range;

        public void Accept(IASTVisitor visitor)
        {
           visitor.Visit(this);
        }

        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        /// <summary>
        /// The local variable name (the "item" in "tablerow item in ..."
        /// </summary>
        public String LocalVariable { get; set; }

        public IIterableCreator IterableCreator { get; set; }

    }
}
