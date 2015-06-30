using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{

    public class GreaterThanExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x > y));
        }
    }

    public class LessThanOrEqualsExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x <= y));
        }
    }

    public class GreaterThanOrEqualsExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x >= y));
        }
    }

    public class LessThanExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack,
            IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var expressionList = expressions.ToList();
            var val1 = expressionList[0].Value;
            var val2 = expressionList[1].Value;
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(val1, val2, (x, y) => x < y));
        }
    }



    public static class ComparisonExpressions
    {
        public static BooleanValue Compare(IExpressionConstant x, IExpressionConstant y,
            Func<decimal, decimal, bool> func)
        {
            var numericValueResult1 = ValueCaster.Cast<IExpressionConstant, NumericValue>(x);
            var numericValueResult2 = ValueCaster.Cast<IExpressionConstant, NumericValue>(y);

            if (numericValueResult2.IsError || numericValueResult1.IsError)
            {
                return new BooleanValue(false);
            }

            var decimalValue1 = numericValueResult1.SuccessValue<NumericValue>().DecimalValue;
            var decimalValue2 = numericValueResult2.SuccessValue<NumericValue>().DecimalValue;
            return new BooleanValue(func(decimalValue1, decimalValue2));
        }

    }
}

