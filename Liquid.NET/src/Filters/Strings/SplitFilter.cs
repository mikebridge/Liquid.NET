using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class SplitFilter : FilterExpression<StringValue, ArrayValue>
    {
        private readonly StringValue _delimiter;

        public SplitFilter(StringValue delimiter)
        {
            _delimiter = delimiter;
        }

        public override ArrayValue ApplyTo(StringValue objectExpression)
        {
            var strings = objectExpression.StringVal.Split(new [] {_delimiter.StringVal}, StringSplitOptions.RemoveEmptyEntries);
            return new ArrayValue(strings.Select(s => (IExpressionConstant) new StringValue(s)).ToList());
        }

    }
}
