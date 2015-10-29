using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class NotExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            IList<Option<IExpressionConstant>> exprList = expressions.ToList();
            if (exprList.Count != 1)
            {
                return LiquidExpressionResult.Error("\"Not\" is a unary expression but received " + exprList.Count + " arguments.");
            }
            return LiquidExpressionResult.Success(new BooleanValue(!exprList[0].HasValue || !exprList[0].Value.IsTrue));
        }
    }
}
