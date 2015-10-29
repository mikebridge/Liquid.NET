using System;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SplitFilter : FilterExpression<StringValue, ArrayValue>
    {
        private readonly StringValue _delimiter;

        public SplitFilter(StringValue delimiter)
        {
            _delimiter = delimiter;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            var strings = liquidStringExpression.StringVal.Split(new[] { _delimiter.StringVal }, StringSplitOptions.RemoveEmptyEntries);
            return LiquidExpressionResult.Success(new ArrayValue(strings.Select(s => new StringValue(s).ToOption()).ToList()));
        }

    }
}
