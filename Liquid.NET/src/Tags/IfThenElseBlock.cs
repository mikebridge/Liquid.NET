using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags 
{
    public class IfThenElseBlock : IASTNode
    {
        // TODO: Fix the names on all these to make them consistent (They're all blocks);
        private readonly IList<IfTagSymbol> _ifExpressions = new List<IfTagSymbol>();

        public IList<IfTagSymbol> IfExpressions { get { return _ifExpressions; } }


        public void AddIfExpression(IfTagSymbol elseSymbol)
        {
            _ifExpressions.Add(elseSymbol);
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

    }

    public class IfTagSymbol
    {
        // the blocks to render if this is true
        public TreeNode<IASTNode> RootContentNode = new TreeNode<IASTNode>(new RootDocumentSymbol());

        // The expression to evaluate
        public TreeNode<ObjectExpression> ObjectExpressionTree { get; set; }

    }

}
