using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression) 
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => x.ToUpper()));
        }
    }
}
