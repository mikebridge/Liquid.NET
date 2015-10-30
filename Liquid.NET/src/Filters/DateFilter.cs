using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    /// <summary>
    /// See: http://www.ruby-doc.org/core/Time.html#method-i-strftime
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DateFilter : FilterExpression<DateValue, StringValue>
    {
        private readonly StringValue _format;

        public DateFilter(StringValue format)
        {
            _format = format;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DateValue dateValue)
        {
            if (/*dateValue.IsUndefined ||*/ !dateValue.DateTimeValue.HasValue)
            {
                return LiquidExpressionResult.Success(new StringValue(""));
            }
            //Console.WriteLine("Formatting " + dateValue.Value + " with format "+_format.StringVal);
            String format = "%m/%d/%Y";
            if (_format != null )
            {
                format = _format.StringVal;
            }
            return LiquidExpressionResult.Success(new StringValue(dateValue.DateTimeValue.Value.ToStrFTime(format)));
            
        }


    }
}
