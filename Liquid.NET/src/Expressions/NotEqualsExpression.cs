using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class NotEqualsExpression :ExpressionDescription
    {
        
//        public override LiquidExpressionResult Accept(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
//        {
//            return LiquidExpressionVisitor.Visit(this, expressions);
//        }
        public override void Accept(ILiquidExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
