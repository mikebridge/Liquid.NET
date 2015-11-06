using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RoundFilter : FilterExpression<LiquidNumeric, LiquidNumeric>
    {
        // ReSharper disable once RedundantDefaultMemberInitializer
        readonly int _decimalPlaces = 0;

        public RoundFilter(LiquidNumeric val)
        {
            if (val != null && val.IntValue > 0)
            {
                _decimalPlaces = val.IntValue;
            }
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidNumeric val)
        {            
            var round = System.Math.Round(val.DecimalValue, _decimalPlaces, MidpointRounding.AwayFromZero);
            return LiquidExpressionResult.Success(_decimalPlaces == 0 ? LiquidNumeric.Create((int) round) : LiquidNumeric.Create(round));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return Apply(ctx, LiquidNumeric.Create(0));
        }
    }
}
