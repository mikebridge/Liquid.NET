using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#camelcase
    /// 
    /// this is Pascal Case, not Camel Case.
    /// </summary>
    public class CamelCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {

            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, before =>
            {
                return String.Concat(before.Split(new[] {' ', '-', '_'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
            }));
        }

    }
}
