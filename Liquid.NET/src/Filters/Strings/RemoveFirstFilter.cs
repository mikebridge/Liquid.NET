using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class RemoveFirstFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _stringToRemove;

        public RemoveFirstFilter(StringValue stringToRemove)
        {
            _stringToRemove = stringToRemove;
        }

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            return StringUtils.Eval(objectExpression, x => StringUtils.ReplaceFirst(x, _stringToRemove.StringVal, ""));            
        }

       
    }
}
