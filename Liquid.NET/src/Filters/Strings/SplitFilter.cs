using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SplitFilter : FilterExpression<LiquidString, LiquidCollection>
    {
        private readonly LiquidString _delimiter;

        public SplitFilter(LiquidString delimiter)
        {
            _delimiter = delimiter;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            var strings = liquidLiquidStringExpression.StringVal.Split(new[] { _delimiter.StringVal }, StringSplitOptions.RemoveEmptyEntries);
            return LiquidExpressionResult.Success(new LiquidCollection(strings.Select(s => new LiquidString(s).ToOption()).ToList()));
        }

    }
}
