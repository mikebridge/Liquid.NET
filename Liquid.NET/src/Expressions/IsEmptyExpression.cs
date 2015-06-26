using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class IsEmptyExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var list = expressions.ToList();
            if (list.Count() != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"empty\"");
            }
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new BooleanValue(true));
            }
            return LiquidExpressionResult.Success(new BooleanValue(EmptyChecker.IsEmpty(list[0].Value)));
        }
    }
}
