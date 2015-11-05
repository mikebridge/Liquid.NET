using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class FalseExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            return LiquidExpressionResult.Success(new Some<IExpressionConstant>(new BooleanValue(false)));
        }
    }
}
