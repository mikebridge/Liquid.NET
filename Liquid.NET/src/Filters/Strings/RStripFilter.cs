using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class RStripFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant liquidExpression)
        {
            return StringUtils.Eval(liquidExpression, x => x.TrimEnd());
        }

    }
}
