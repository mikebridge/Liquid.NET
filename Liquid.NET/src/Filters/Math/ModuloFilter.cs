using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModuloFilter: FilterExpression<LiquidNumeric, LiquidNumeric>
    {
        private readonly LiquidNumeric _operand;

        public ModuloFilter(LiquidNumeric operand) { _operand = operand; }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric liquidNumeric)
        {
            if (_operand == null)
            {
                return LiquidExpressionResult.Error("The argument to \"modulo\" is missing.");
            }
            if (_operand.DecimalValue == 0)
            {
                return LiquidExpressionResult.Error("Liquid error: divided by 0");
            }
            var val = (int) (liquidNumeric.DecimalValue % _operand.DecimalValue);
            return LiquidExpressionResult.Success(LiquidNumeric.Create(val));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(0));
        }
    }
}
