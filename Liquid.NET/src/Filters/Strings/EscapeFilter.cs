using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#escape
    /// </summary>
    public class EscapeFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            return StringUtils.Eval(objectExpression, WebUtility.HtmlEncode);
        }
    }
}
