using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression) 
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => x.ToUpper()));
        }
    }
}
