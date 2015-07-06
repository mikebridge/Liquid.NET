using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#newline_to_br
    /// </summary>
    public class NewlineToBrFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public const String BR = "<br />\r\n";

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => Regex.Replace(x, @"(\r\n)|[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]", BR)));
        }

    }
}
