using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class TruncateFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly NumericValue _length;
        private readonly StringValue _truncateString;

        public TruncateFilter(NumericValue length, StringValue truncateString)
        {
            _length = length;
            if (truncateString.IsUndefined)
            {
                _truncateString = new StringValue("...");
            }
            else
            {
                _truncateString = truncateString;
            }
        }

        public override StringValue ApplyTo(IExpressionConstant objectExpression)
        {
            return StringUtils.Eval(objectExpression, Truncate);
        }

        private string Truncate(string s)
        {
            int requestedLength = _length.IntValue;
            String ellipsis = _truncateString.StringVal;
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
