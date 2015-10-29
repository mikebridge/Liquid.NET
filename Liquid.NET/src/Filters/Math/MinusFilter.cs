using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MinusFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public MinusFilter(NumericValue operand)
        {
            _operand = operand;
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue liquidExpression)
        {
            if (_operand == null)
            {
                return LiquidExpressionResult.Error("The operand to \"" + Name + "\" is missing.");
            }
            var result = liquidExpression.DecimalValue - _operand.DecimalValue;
            return MathHelper.GetReturnValue(result, liquidExpression, _operand);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            var dec = _operand == null ? 0m : _operand.DecimalValue;
            return MathHelper.GetReturnValue(- dec, NumericValue.Create(0), _operand);
        }

    }
}
