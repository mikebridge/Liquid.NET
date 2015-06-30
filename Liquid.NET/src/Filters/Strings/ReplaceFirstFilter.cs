using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class ReplaceFirstFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _stringToRemove;
        private readonly StringValue _replacementString;

        public ReplaceFirstFilter(StringValue stringToRemove, StringValue replacementString)
        {
            _stringToRemove = stringToRemove ?? new StringValue(""); 
            _replacementString = replacementString ?? new StringValue("");
        }

        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => StringUtils.ReplaceFirst(x, _stringToRemove.StringVal, _replacementString.StringVal)));
        }


    }
}
