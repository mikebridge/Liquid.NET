using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TruncateWordsFilter : FilterExpression<LiquidString, LiquidString>
    {
        private LiquidNumeric _length;
        private readonly LiquidString _truncateLiquidString;

        public TruncateWordsFilter(LiquidNumeric length, LiquidString truncateLiquidString)
        {
            _length = length;
            _truncateLiquidString = truncateLiquidString == null || truncateLiquidString.Value == null ? new LiquidString("...") : truncateLiquidString;
        }
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidLiquidStringExpression, TruncateWords));
            
        }

        private string TruncateWords(string s)
        {
            if (s == null)
            {
                return "";
            }
            if (_length == null)
            {
                _length = LiquidNumeric.Create(15);
            }
            var words = s.Split(new [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= _length.IntValue)
            {
                return s;
            }
            // this removes rendundant spaces.
            return String.Join(" ", words.Take(_length.IntValue)) + _truncateLiquidString.StringVal;
        }
    }
}
