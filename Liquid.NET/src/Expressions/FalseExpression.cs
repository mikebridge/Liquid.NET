using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class FalseExpression : ExpressionDescription
    {
//        public override LiquidExpressionResult Accept(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
//        {
//            return LiquidExpressionVisitor.Visit(this);
//        }

        public override void Accept(ILiquidExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
