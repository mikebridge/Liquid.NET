using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class TruncateWordsFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            //return StringResult.Eval(objectExpression, x => );
            throw new NotImplementedException();
        }

    }
}
