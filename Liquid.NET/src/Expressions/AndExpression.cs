using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class AndExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var exprList = expressions.ToList();
            if (exprList.Count() != 2)
            {
                throw new Exception("An AND expression must have two values"); // TODO: when the Eval is separated this will be redundant.
            }
            return LiquidExpressionResult.Success(new BooleanValue(exprList.All(x => x.HasValue) && exprList.All(x => x.Value.IsTrue)));
        }
    }
}
