using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    public class DefaultFilter : FilterExpression<IExpressionConstant, IExpressionConstant>
    {
        private readonly IExpressionConstant _defaultValue;

        public DefaultFilter(IExpressionConstant defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(liquidExpression.ToOption());
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            Option<IExpressionConstant> result = _defaultValue == null
                ? (Option<IExpressionConstant>) new None<IExpressionConstant>()
                : new Some<IExpressionConstant>(_defaultValue);
            return  LiquidExpressionResult.Success(result);
        }
    }
}
