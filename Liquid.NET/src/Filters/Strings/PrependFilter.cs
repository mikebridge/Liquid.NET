using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class PrependFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _prependedStr;

        public PrependFilter(StringValue prependedStr)
        {
            _prependedStr = prependedStr;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            var strToPrepend = _prependedStr == null ? "" : _prependedStr.StringVal;
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => strToPrepend + x));
        }
    }
}
