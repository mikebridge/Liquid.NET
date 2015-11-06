using System.Text.RegularExpressions;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StripNewlinesFilter : FilterExpression<ILiquidValue, LiquidString>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => Regex.Replace(x, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", "").Trim()));
        }
        
    }
}
