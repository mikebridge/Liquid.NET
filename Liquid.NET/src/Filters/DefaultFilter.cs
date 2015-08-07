using System;
using Liquid.NET.Constants;
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

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue arrayValue)
        {
            if (arrayValue != null && arrayValue.ArrValue.Count > 0)
            {
                return LiquidExpressionResult.Success(arrayValue.ToOption());
            }
            else
            {
                return CreateDefaultReturn();
            }
        }


        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return CreateDefaultReturn();
        }

        private LiquidExpressionResult CreateDefaultReturn()
        {
            Option<IExpressionConstant> result = _defaultValue == null
                ? (Option<IExpressionConstant>) new None<IExpressionConstant>()
                : new Some<IExpressionConstant>(_defaultValue);
            return LiquidExpressionResult.Success(result);
        }
    }
}
