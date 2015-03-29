using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override StringValue ApplyTo(IExpressionConstant liquidExpression) 
        {
            return StringUtils.Eval(liquidExpression, x => x.ToUpper());
        }
    }
}
