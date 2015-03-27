using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class ReplaceFirstFilter : FilterExpression<IExpressionConstant, StringValue>
    {
       private readonly StringValue _stringToRemove;
       private readonly StringValue _replacementString;

        public ReplaceFirstFilter(StringValue stringToRemove, StringValue replacementString)
        {
            _stringToRemove = stringToRemove;
            _replacementString = replacementString;
        }

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            return StringUtils.Eval(objectExpression, x => StringUtils.ReplaceFirst(x, _stringToRemove.StringVal, _replacementString.StringVal));
        }


    }
}
