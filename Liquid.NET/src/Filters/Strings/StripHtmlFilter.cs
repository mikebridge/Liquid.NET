using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class StripHtmlFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            throw new NotImplementedException();
        }
    }
}
