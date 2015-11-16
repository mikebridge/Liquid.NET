using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.ILiquidValue>>;

namespace Liquid.NET.Expressions
{
    public class AndExpression : ExpressionDescription
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
