using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FloorFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric val)
        {
            var floor = (int) System.Math.Floor(val.DecimalValue);
            return LiquidExpressionResult.Success(LiquidNumeric.Create(floor));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(0));
        }
    }
}
