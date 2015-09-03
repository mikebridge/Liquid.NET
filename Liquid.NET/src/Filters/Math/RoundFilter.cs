using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class RoundFilter : FilterExpression<NumericValue, NumericValue>
    {
        readonly int decimalPlaces = 0;

        public RoundFilter(NumericValue val)
        {
            if (val != null && /*!val.IsUndefined && */ val.IntValue > 0)
            {
                decimalPlaces = val.IntValue;
            }
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue val)
        {            
            var round = System.Math.Round(val.DecimalValue, decimalPlaces, MidpointRounding.AwayFromZero);
            return LiquidExpressionResult.Success(decimalPlaces == 0 ? NumericValue.Create((int) round) : NumericValue.Create(round));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return Apply(ctx, NumericValue.Create(0));
        }
    }
}
