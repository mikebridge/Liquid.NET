using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#append
    /// </summary>
    public class AppendFilter : FilterExpression<StringValue, StringValue>
    {
        private readonly StringValue _strToAppend;

        public AppendFilter(StringValue strToAppend)
        {
            _strToAppend = strToAppend;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidExpression)
        {
            return LiquidExpressionResult.Success(new StringValue(liquidExpression.StringVal + _strToAppend.StringVal)); 
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return ApplyTo(ctx, new StringValue(""));
        }

    }
}
