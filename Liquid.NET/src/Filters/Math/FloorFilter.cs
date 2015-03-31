using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class FloorFilter : FilterExpression<NumericValue, NumericValue>
    {

        public override NumericValue Apply(NumericValue val)
        {
            return new NumericValue(System.Math.Floor(val.DecimalValue));
        }
    }
}
