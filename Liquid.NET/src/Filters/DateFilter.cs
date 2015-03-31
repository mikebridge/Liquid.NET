using System;
using Liquid.NET.Utils;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters
{
    /// <summary>
    /// See: http://www.ruby-doc.org/core/Time.html#method-i-strftime
    /// </summary>
    public class DateFilter : FilterExpression<DateValue, StringValue>
    {
        private readonly StringValue _format;

        public DateFilter(StringValue format)
        {
            _format = format;
        }

        public override StringValue ApplyTo(DateValue dateValue)
        {
            if (dateValue.IsUndefined || !dateValue.DateTimeValue.HasValue)
            {
                return new StringValue("");
            }
            Console.WriteLine("Formatting " + dateValue.Value + " with format "+_format.StringVal);
            return new StringValue(dateValue.DateTimeValue.Value.ToStrFTime(_format.StringVal));
            
        }


    }
}
