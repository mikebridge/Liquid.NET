using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class CeilFilter : FilterExpression<NumericValue, NumericValue>
    {
        public override NumericValue Apply(NumericValue val)
        {
            return new NumericValue(System.Math.Ceiling(val.DecimalValue));
        }
    }
}
