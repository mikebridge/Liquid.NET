using System.Collections.Generic;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    /// <summary>
    /// The difference between case expressions and if/unless expressions is that
    /// while the if/else/elsif expressions return a boolean value (and an "else" just 
    /// returns true, since it always matches), a case expression might be any kind of 
    /// value.  That value gets compared to the CaseWhenElseBlockTag.LiquidExpressionTree
    /// result.
    /// </summary>
    public class CaseWhenElseBlockTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        private readonly IList<WhenClause> _whenClauses = new List<WhenClause>();

        public IList<WhenClause> WhenClauses { get { return _whenClauses; } }

        public WhenElseClause ElseClause { get; set; }

        public void AddWhenBlock(WhenClause elseSymbol)
        {
            _whenClauses.Add(elseSymbol);
        }

        /// <summary>
        /// TODO: think of a better name for this.  "Thing to match case
        /// results to".
        /// </summary>
        public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

        public bool HasElseClause
        {
            get { return ElseClause != null; }
            
        }

        public class WhenClause
        {
            // the blocks to render when this WHEN expression result matches the where 
            public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());

            // The expression to evaluate
            public TreeNode<LiquidExpression> LiquidExpressionTree { get; set; }

        }

        public class WhenElseClause
        {
            public TreeNode<IASTNode> LiquidBlock = new TreeNode<IASTNode>(new RootDocumentNode());
        }

    }
}
