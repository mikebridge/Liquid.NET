using System;
using System.Text.RegularExpressions;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class StripNewlinesFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => Regex.Replace(x, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", "").Trim()));
        }
        
    }
}
