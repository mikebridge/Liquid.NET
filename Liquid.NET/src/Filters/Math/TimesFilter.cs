using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TimesFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {
        private readonly LiquidNumeric _addend1;

        public TimesFilter(LiquidNumeric addend1)
        {
            _addend1 = addend1;
        }


        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidNumeric addend2)
        {
            var result = _addend1.DecimalValue * addend2.DecimalValue;
            return MathHelper.GetReturnValue(result, _addend1, addend2);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(0)); 
        }
    }
}
