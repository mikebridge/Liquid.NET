using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#escape
    /// </summary>
    public class EscapeFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, WebUtility.HtmlEncode));
        }
    }
}
