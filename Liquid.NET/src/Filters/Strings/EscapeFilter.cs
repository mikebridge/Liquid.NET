using System.Net;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#escape
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EscapeFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, WebUtility.HtmlEncode));
        }
    }
}
