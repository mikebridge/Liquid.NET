using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToIntFilter : FilterExpression<LiquidValue, LiquidNumeric>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(ToDecimalFilter.ConvertToDecimal(liquidExpression).IntValue));
        }
    }
}
