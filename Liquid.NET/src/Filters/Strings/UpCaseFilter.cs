using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<StringValue, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidExpression) 
        {
            return LiquidExpressionResult.Success(liquidExpression.ToString().ToUpper());
        }
    }
}
