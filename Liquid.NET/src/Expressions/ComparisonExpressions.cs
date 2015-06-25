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
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value, (x, y) => x < y));
        }
    }



    public static class ComparisonExpressions
    {
        public static BooleanValue Compare(IExpressionConstant x, IExpressionConstant y,
            Func<decimal, decimal, bool> func)
        {
            var numericValue1 = ValueCaster.Cast<IExpressionConstant, NumericValue>(x);
            var numericValue2 = ValueCaster.Cast<IExpressionConstant, NumericValue>(y);

            return new BooleanValue(func(numericValue1.DecimalValue, numericValue2.DecimalValue));
        }

    }
}

