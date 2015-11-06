using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{

    public class GreaterThanExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x > y));
        }
    }

    public class LessThanOrEqualsExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x <= y));
        }
    }

    public class GreaterThanOrEqualsExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Eval(ITemplateContext templateContext,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x >= y));
        }
    }

    public class LessThanExpression : ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            
            var val1 = expressionList[0].Value;
            var val2 = expressionList[1].Value;
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(val1, val2, (x, y) => x < y));
        }
    }



    public static class ComparisonExpressions
    {
        public static LiquidBoolean Compare(IExpressionConstant x, IExpressionConstant y,
            Func<decimal, decimal, bool> func)
        {

            var numericValueResult1 = ValueCaster.Cast<IExpressionConstant, LiquidNumeric>(x);
            var numericValueResult2 = ValueCaster.Cast<IExpressionConstant, LiquidNumeric>(y);

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

