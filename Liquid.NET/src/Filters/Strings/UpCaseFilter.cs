using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        public override StringValue ApplyTo(IExpressionConstant objectExpression) 
        {
            return StringResult.Eval(objectExpression, x => x.ToLower());
        }
    }
}
