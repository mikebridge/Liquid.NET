using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{

    public class GreaterThanExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Accept(ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionVisitor.Visit(this, expressions);
        }
    }

    public class LessThanOrEqualsExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Accept(ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionVisitor.Visit(this, expressions);
        }
    }

    public class GreaterThanOrEqualsExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Accept(ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionVisitor.Visit(this, expressions);
        }
    }

    public class LessThanExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Accept(ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionVisitor.Visit(this, expressions);
        }
    }



    public static class ComparisonExpressions
    {
        public static LiquidBoolean Compare(ILiquidValue x, ILiquidValue y,
            Func<decimal, decimal, bool> func)
        {

            var numericValueResult1 = ValueCaster.Cast<ILiquidValue, LiquidNumeric>(x);
            var numericValueResult2 = ValueCaster.Cast<ILiquidValue, LiquidNumeric>(y);

            if (numericValueResult2.IsError || numericValueResult1.IsError)
            {
                return new LiquidBoolean(false);
            }

            var decimalValue1 = numericValueResult1.SuccessValue<LiquidNumeric>().DecimalValue;
            var decimalValue2 = numericValueResult2.SuccessValue<LiquidNumeric>().DecimalValue;
            return new LiquidBoolean(func(decimalValue1, decimalValue2));
        }

    }
}

