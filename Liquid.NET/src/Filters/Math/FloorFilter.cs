using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class FloorFilter : FilterExpression<NumericValue, NumericValue>
    {

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue val)
        {
            var floor = (int) System.Math.Floor(val.DecimalValue);
            return LiquidExpressionResult.Success(NumericValue.Create(floor));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(NumericValue.Create(0));
        }
    }
}
