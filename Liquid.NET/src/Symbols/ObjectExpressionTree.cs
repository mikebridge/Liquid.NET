using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class ObjectExpressionTree : IASTNode
    {

        public ObjectExpressionTree(TreeNode<ObjectExpression> objectExpressionTree)
        {
            ExpressionTree = objectExpressionTree;
        }

        public TreeNode<ObjectExpression> ExpressionTree { get; private set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
