using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#camelcase
    /// </summary>
    public class CamelCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
         private readonly StringValue _strToAppend;

         public CamelCaseFilter(StringValue strToAppend)
        {
            _strToAppend = strToAppend;
        }

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            //return new StringValue(ValueCaster.RenderAsString(objectExpression) + _strToAppend.StringVal);
            throw new NotImplementedException();
        }

    }
}
