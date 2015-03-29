using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class MinusFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public MinusFilter(NumericValue operand)
        {
            _operand = operand;
        }

        public override NumericValue Apply(NumericValue liquidExpression)
        {
            return new NumericValue(liquidExpression.DecimalValue - _operand.DecimalValue);
        }
    }
}
