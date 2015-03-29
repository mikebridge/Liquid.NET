using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class AssignTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public String VarName { get; set; }

        public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

    }
}
