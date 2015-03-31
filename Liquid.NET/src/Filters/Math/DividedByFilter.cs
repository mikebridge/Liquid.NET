using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class DividedByFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _divisor;

        public DividedByFilter(NumericValue divisor)
        {
            _divisor = divisor;
        }

        public override NumericValue Apply(NumericValue dividend)
        {
            if (dividend == null)
            {
                return NumericValue.CreateError("The dividend is missing.");
            }
            if (_divisor == null)
            {
                return NumericValue.CreateError("The divisor is missing.");
            }
            if (_divisor.DecimalValue == 0)
            {
                return NumericValue.CreateError("Divide-by-zero error.");
            }
            return new NumericValue(dividend.DecimalValue / _divisor.DecimalValue );
        }
    }

}
