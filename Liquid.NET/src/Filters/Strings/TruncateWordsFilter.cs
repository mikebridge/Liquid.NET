using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TruncateWordsFilter : FilterExpression<StringValue, StringValue>
    {
        private NumericValue _length;
        private readonly StringValue _truncateString;

        public TruncateWordsFilter(NumericValue length, StringValue truncateString)
        {
            _length = length;
            _truncateString = truncateString == null || truncateString.Value == null ? new StringValue("...") : truncateString;
        }
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidStringExpression, TruncateWords));
            
        }

        private string TruncateWords(string s)
        {
            if (s == null)
            {
                return "";
            }
            if (_length == null)
            {
                _length = NumericValue.Create(15);
            }
            var words = s.Split(new [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= _length.IntValue)
            {
                return s;
            }
            // this removes rendundant spaces.
            return String.Join(" ", words.Take(_length.IntValue)) + _truncateString.StringVal;
        }
    }
}
