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
            if (_delimiter == null)
            {
                return LiquidExpressionResult.Error("Split filter must have a delimiter");
            }
            var strings = liquidLiquidStringExpression.StringVal.Split(new[] { _delimiter.StringVal }, StringSplitOptions.RemoveEmptyEntries);
            return LiquidExpressionResult.Success(new LiquidCollection(strings.Select(s => LiquidString.Create(s).ToOption()).ToList()));
        }

    }
}
