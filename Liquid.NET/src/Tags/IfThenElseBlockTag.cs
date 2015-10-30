using System.Collections.Generic;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags 
{
    public class IfThenElseBlockTag : IASTNode
    {
        private readonly IList<IfElseClause> _ifElseClauses = new List<IfElseClause>();

        public IList<IfElseClause> IfElseClauses { get { return _ifElseClauses; } }


        public void AddIfClause(IfElseClause elseSymbol)
        {
            _ifElseClauses.Add(elseSymbol);
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

    }

    public class IfElseClause
    {
        // the liquid blocks to render if this is true
        public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

        // The expression to evaluate
        public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

    }

}
