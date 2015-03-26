using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<StringValue, StringValue>
    {

        public override StringValue Apply(StringValue stringExpression)
        {
            // TODO: parameterize the type
            String val = (String) stringExpression.Value;
            return new StringValue(val.ToUpper());
        }
    }
}
