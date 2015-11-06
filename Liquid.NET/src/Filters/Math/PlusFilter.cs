using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    //public class PlusFilter : FilterExpression<NumericLiteral, NumericLiteral>
    public class PlusFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {
        private readonly LiquidNumeric _operand;

        public PlusFilter(LiquidNumeric operand) { _operand = operand; }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric liquidNumeric)
        {
            if (_operand == null)
            {
                return LiquidExpressionResult.Error("The argument to \""+Name+"\" is missing.");
            }
            var val = liquidNumeric.DecimalValue +  _operand.DecimalValue;

            return MathHelper.GetReturnValue(val, liquidNumeric, _operand);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(_operand);
        }
    }
}
