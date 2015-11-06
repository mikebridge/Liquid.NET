using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#append
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AppendFilter : FilterExpression<LiquidString, LiquidString>
    {
        private readonly LiquidString _strToAppend;

        public AppendFilter(LiquidString strToAppend)
        {
            _strToAppend = strToAppend;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidExpression)
        {
            var strToAppend = _strToAppend == null ? "" : _strToAppend.StringVal;
            return LiquidExpressionResult.Success(new LiquidString(liquidExpression.StringVal + strToAppend));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return ApplyTo(ctx, new LiquidString(""));
        }

    }
}
