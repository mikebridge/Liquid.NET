using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class RoundFilter : FilterExpression<NumericValue, NumericValue>
    {
        int decimalPlaces = 0;

        public RoundFilter(NumericValue val)
        {
            if (val != null && !val.IsUndefined && val.IntValue > 0)
            {
                decimalPlaces = val.IntValue;
            }
        }

        public override NumericValue Apply(NumericValue val)
        {
            return new NumericValue(System.Math.Round(val.DecimalValue, decimalPlaces, MidpointRounding.AwayFromZero));
        }
    }
}
