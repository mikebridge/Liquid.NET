using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DividedByFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {        
        private readonly LiquidNumeric _divisor;

        public DividedByFilter(LiquidNumeric divisor)
        {
            _divisor = divisor;
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric dividend)
        {
            if (dividend == null)
            {
                return LiquidExpressionResult.Error("The dividend is missing.");
            }
            if (_divisor == null)
            {
                return LiquidExpressionResult.Error("The divisor is missing.");
            }
            if (_divisor.DecimalValue == 0.0m)
            {
                return LiquidExpressionResult.Error("Liquid error: divided by 0");
            }
            var result = dividend.DecimalValue / _divisor.DecimalValue;
            return MathHelper.GetReturnValue(result, dividend, _divisor);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return Apply(ctx, LiquidNumeric.Create(0));

        }
    }

}
