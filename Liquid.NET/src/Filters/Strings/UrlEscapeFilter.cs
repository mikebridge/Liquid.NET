using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UrlEscapeFilter : FilterExpression<ILiquidValue, LiquidString>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            //return StringUtils.Eval(liquidStringExpression, x => WebUtility.UrlEncode(x));
            // Dunno if this is right but it seems to do the same as CGI::escape
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, Uri.EscapeUriString));
        }

    }
}
