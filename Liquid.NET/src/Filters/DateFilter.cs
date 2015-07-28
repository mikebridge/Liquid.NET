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
        private readonly StringValue _culture;

        public DateFilter(StringValue format, StringValue culture = null)
        {
            _format = format;
            _culture = culture;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DateValue dateValue)
        {
            if (/*dateValue.IsUndefined ||*/ !dateValue.DateTimeValue.HasValue)
            {
                return LiquidExpressionResult.Success(new StringValue(""));
            }

            var output = Format(dateValue);

            return output;
        }

        private LiquidExpressionResult Format(DateValue dateValue)
        {
            if (_culture == null)
            {
                return FormatSimple(dateValue);
            }
            else
            {
                return FormatWithCulture(dateValue);
            }
        }

        private LiquidExpressionResult FormatSimple(DateValue dateValue)
        {
            Console.WriteLine("Formatting " + dateValue.Value + " with format " + _format.StringVal);
            return
                LiquidExpressionResult.Success(
                    new StringValue(dateValue.DateTimeValue.Value.ToStrFTime(_format.StringVal)));
        }

        private LiquidExpressionResult FormatWithCulture(DateValue dateValue)
        {
            Console.WriteLine("Formatting " + dateValue.Value + " with format " + _format.StringVal + " with culture " + _culture.Value.ToString());

            var oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_culture.Value.ToString());
                return
                LiquidExpressionResult.Success(
                    new StringValue(dateValue.DateTimeValue.Value.ToStrFTime(_format.StringVal)));
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }
    }
}
