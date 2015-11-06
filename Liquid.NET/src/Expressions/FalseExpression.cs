using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class FalseExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionResult.Success(new Some<ILiquidValue>(new LiquidBoolean(false)));
        }
    }
}
