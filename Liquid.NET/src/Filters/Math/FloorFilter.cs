using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class FloorFilter : FilterExpression<NumericValue, NumericValue>
    {

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue val)
        {
            var floor = (int) System.Math.Floor(val.DecimalValue);
            return LiquidExpressionResult.Success(new NumericValue(floor));
        }
    }
}
