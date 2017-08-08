using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TruncateFilter : FilterExpression<ILiquidValue, LiquidString>
    {
        private readonly LiquidNumeric _length;
        private readonly LiquidString _truncateLiquidString;

        public TruncateFilter(LiquidNumeric length, LiquidString truncateLiquidString)
        {
            _length = length;
            _truncateLiquidString = truncateLiquidString == null || truncateLiquidString.Value == null ? LiquidString.Create("...") : truncateLiquidString;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, Truncate));
        }

        private string Truncate(string s)
        {
            int requestedLength = _length.IntValue;
            String ellipsis = _truncateLiquidString.StringVal;
            if (s == null)
            {
                return "";
            }
            
            // return the string if it doesn't need the ellipsis
            if (s.Length < requestedLength)
            {
                return s;
            }
            // return s if it's shorter than the ellipsis;
            if (ellipsis.Length >= requestedLength)
            {
                return s.Substring(0, requestedLength );
            }
          
            
            return s.Substring(0, requestedLength - ellipsis.Length) + ellipsis;
        }
    }
}
