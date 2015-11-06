using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class NotExpression : ExpressionDescription
    {
//        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            expressionDescriptionVisitor.Visit(this);
//        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
        {
            IList<Option<ILiquidValue>> exprList = expressions.ToList();
            if (exprList.Count != 1)
            {
                return LiquidExpressionResult.Error("\"Not\" is a unary expression but received " + exprList.Count + " arguments.");
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(!exprList[0].HasValue || !exprList[0].Value.IsTrue));
        }
    }
}
