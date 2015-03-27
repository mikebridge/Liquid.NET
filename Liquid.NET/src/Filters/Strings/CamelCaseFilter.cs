using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#camelcase
    /// 
    /// this is Pascal Case, not Camel Case.
    /// </summary>
    public class CamelCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {

            return StringResult.Eval(objectExpression, before =>
            {
                return String.Concat(before.Split(new[] {' ', '-', '_'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
            });
        }

    }
}
