using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModuloFilter: FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public ModuloFilter(NumericValue operand) { _operand = operand; }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue numericValue)
        {
            if (_operand == null)
            {
                return LiquidExpressionResult.Error("The argument to \"modulo\" is missing.");
            }
            if (_operand.DecimalValue == 0)
            {
                return LiquidExpressionResult.Error("Liquid error: divided by 0");
            }
            var val = (int) (numericValue.DecimalValue % _operand.DecimalValue);
            return LiquidExpressionResult.Success(NumericValue.Create(val));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(NumericValue.Create(0));
        }
    }
}
