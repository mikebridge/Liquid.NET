using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    /// <summary>
    /// See: http://www.ruby-doc.org/core/Time.html#method-i-strftime
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DateFilter : FilterExpression<LiquidDate, LiquidString>
    {
        private readonly LiquidString _format;

        public DateFilter(LiquidString format)
        {
            _format = format;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidDate liquidDate)
        {
            if (/*dateValue.IsUndefined ||*/ !liquidDate.DateTimeValue.HasValue)
            {
                return LiquidExpressionResult.Success(LiquidString.Create(""));
            }
            //Console.WriteLine("Formatting " + dateValue.Value + " with format "+_format.StringVal);
            String format = "%m/%d/%Y";
            if (_format != null )
            {
                format = _format.StringVal;
            }
            return LiquidExpressionResult.Success(LiquidString.Create(liquidDate.DateTimeValue.Value.ToStrFTime(format)));
            
        }


    }
}
