using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class MinusFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public MinusFilter(NumericValue operand)
        {
            _operand = operand;
        }

        public override LiquidExpressionResult Apply(NumericValue liquidExpression)
        {
            return LiquidExpressionResult.Success(new NumericValue(liquidExpression.DecimalValue - _operand.DecimalValue));
        }
    }
}
