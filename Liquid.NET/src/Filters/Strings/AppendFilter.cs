using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#append
    /// </summary>
    public class AppendFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _strToAppend;

        public AppendFilter(StringValue strToAppend)
        {
            _strToAppend = strToAppend;
        }

        public override StringValue ApplyTo(IExpressionConstant liquidExpression)
        {
            return StringUtils.Eval(liquidExpression, x => x + _strToAppend.StringVal);            
        }
    }
}
