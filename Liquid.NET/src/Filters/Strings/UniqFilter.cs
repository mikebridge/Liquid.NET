using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UniqFilter : FilterExpression<ArrayValue, ArrayValue>
    {

        public override LiquidExpressionResult ApplyTo(ArrayValue liquidArrayExpression)
        {
            return LiquidExpressionResult.Success(new ArrayValue(liquidArrayExpression.ArrValue.Distinct(new EasyValueComparer()).ToList()));
            
        }

    }
}
