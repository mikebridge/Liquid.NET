using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToIntFilter : FilterExpression<ExpressionConstant, NumericValue>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, ExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(NumericValue.Create(ToDecimalFilter.ConvertToDecimal(liquidExpression).IntValue));
        }
    }
}
