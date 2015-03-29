using System;

using System.Linq;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    // TODO: this should work on an array.
    public class JoinFilter : FilterExpression<ExpressionConstant, StringValue>
    {
        private readonly StringValue _separator;

        public JoinFilter(StringValue separator)
        {
            _separator = separator;
        }

        public override StringValue ApplyTo(ArrayValue liquidArrayExpression)
        {
            var vals = liquidArrayExpression
                .ArrValue
                .Select(ValueCaster.Cast<IExpressionConstant, StringValue>)
                .Select(x => x.Value);
            return new StringValue(String.Join(_separator.StringVal, vals));
        }

        public override StringValue ApplyTo(StringValue liquidStringExpression)
        {
            if (String.IsNullOrEmpty(liquidStringExpression.StringVal))
            {
                return new StringValue(null);
            }
            return new StringValue(String.Join(_separator.StringVal,
                liquidStringExpression.StringVal.ToCharArray().Select(c => c.ToString())));

        }
    }
}
