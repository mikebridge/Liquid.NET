using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class RoundFilter : FilterExpression<NumericValue, NumericValue>
    {
        readonly int decimalPlaces = 0;

        public RoundFilter(NumericValue val)
        {
            if (val != null && /*!val.IsUndefined && */ val.IntValue > 0)
            {
                decimalPlaces = val.IntValue;
            }
        }

        public override LiquidExpressionResult Apply(NumericValue val)
        {            
            var round = System.Math.Round(val.DecimalValue, decimalPlaces, MidpointRounding.AwayFromZero);
            return LiquidExpressionResult.Success(decimalPlaces == 0 ? new NumericValue((int) round) : new NumericValue(round));
        }
    }
}
