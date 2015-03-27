using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override StringValue ApplyTo(IExpressionConstant objectExpression) 
        {
            return StringUtils.Eval(objectExpression, x => x.ToUpper());
        }
    }
}
