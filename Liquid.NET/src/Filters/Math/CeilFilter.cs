using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CeilFilter : FilterExpression<NumericValue, NumericValue>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue val)
        {
            var ceiling = (int) System.Math.Ceiling(val.DecimalValue);

            return LiquidExpressionResult.Success(NumericValue.Create(ceiling));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(NumericValue.Create(0));
        }
    }
}
