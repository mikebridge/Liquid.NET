using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class PrependFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _prependedStr;

        public PrependFilter(StringValue prependedStr)
        {
            _prependedStr = prependedStr;
        }

        public override StringValue ApplyTo(IExpressionConstant liquidExpression)
        {
            return StringUtils.Eval(liquidExpression, x => _prependedStr.StringVal + x);
        }
    }
}
