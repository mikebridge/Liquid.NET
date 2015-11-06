using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CeilFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric val)
        {
            var ceiling = (int) System.Math.Ceiling(val.DecimalValue);

            return LiquidExpressionResult.Success(LiquidNumeric.Create(ceiling));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(0));
        }
    }
}
