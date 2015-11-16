using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class VariableReferenceTree : ExpressionDescription
    {

        public VariableReferenceTree Parent { get; set; }

        public IExpressionDescription Value { get; set; }

        public VariableReferenceTree IndexExpression { get; set; }
        
//        public LiquidExpressionResult Accept(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
//        {
//            return LiquidExpressionVisitor.Visit(this, templateContext, childresults).LiquidExpressionResult;
//        }

        public override void Accept(ILiquidExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
