using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#pluralize
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PluralizeFilter : FilterExpression<LiquidNumeric, LiquidString>
    {
        private readonly LiquidString _single;
        private LiquidString _plural;

        public PluralizeFilter(LiquidString single, LiquidString plural)
        {
            _single = single;
            _plural = plural;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidNumeric liquidNumeric)
        {
            String numericString = ValueCaster.RenderAsString((IExpressionConstant) liquidNumeric);
            if (_single == null && _plural == null)
            {
                return LiquidExpressionResult.Success(new LiquidString(numericString));
            }
            if (_plural == null)
            {
                _plural = new LiquidString("");
            }
            var str = new LiquidString(numericString+" ");
            return LiquidExpressionResult.Success(str.Join(liquidNumeric.DecimalValue == 1 ? _single : _plural));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return ApplyTo(ctx, LiquidNumeric.Create(0));
        }
    }
}
