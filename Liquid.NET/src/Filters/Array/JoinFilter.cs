using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class JoinFilter : FilterExpression<ILiquidValue, LiquidString>
    {
        private readonly LiquidString _separator;

        public JoinFilter(LiquidString separator)
        {
            _separator = separator;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
        {
            String separator = _separator == null ? "" : _separator.StringVal;

            var vals = liquidArrayExpression.Select(ValueCaster.RenderAsString);

            return LiquidExpressionResult.Success(LiquidString.Create(String.Join(separator, vals)));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            if (String.IsNullOrEmpty(liquidLiquidStringExpression.StringVal))
            {
                return LiquidExpressionResult.Success(Option<ILiquidValue>.None());
            }

            return LiquidExpressionResult.Success(LiquidString.Create(String.Join(_separator.StringVal,
                liquidLiquidStringExpression.StringVal.ToCharArray().Select(c => c.ToString()))));

        }
    }
}
