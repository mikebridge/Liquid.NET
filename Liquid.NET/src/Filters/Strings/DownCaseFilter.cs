using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#downcase
    /// </summary>
    public class DownCaseFilter: FilterExpression<IExpressionConstant, StringValue>
    {
        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            return StringUtils.Eval(objectExpression, x => x.ToLower());
        }
    }
}
