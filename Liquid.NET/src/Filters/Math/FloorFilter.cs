using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class FloorFilter : FilterExpression<NumericValue, NumericValue>
    {

        public override LiquidExpressionResult Apply(NumericValue val)
        {
            return new NumericValue(System.Math.Floor(val.DecimalValue));
        }
    }
}
