using System;

using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

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

        public override LiquidExpressionResult ApplyTo(ArrayValue liquidArrayExpression)
        {
            var vals = liquidArrayExpression
                .ArrValue
                .Select(ValueCaster.Cast<IExpressionConstant, StringValue>)
                .Select(x => x.Value);
            return LiquidExpressionResult.Success(new StringValue(String.Join(_separator.StringVal, vals)));
        }

        public override LiquidExpressionResult ApplyTo(StringValue liquidStringExpression)
        {
            if (String.IsNullOrEmpty(liquidStringExpression.StringVal))
            {
                return LiquidExpressionResult.Success(new StringValue(null));
            }
            return LiquidExpressionResult.Success(new StringValue(String.Join(_separator.StringVal,
                liquidStringExpression.StringVal.ToCharArray().Select(c => c.ToString()))));

        }
    }
}
