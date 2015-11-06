using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DefaultFilter : FilterExpression<ILiquidValue, ILiquidValue>
    {
        private readonly ILiquidValue _defaultValue;

        public DefaultFilter(ILiquidValue defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidExpression)
        {
            if (liquidExpression.StringVal == null || liquidExpression.StringVal.Equals(""))
            {
                return CreateDefaultReturn();
            }
            else
            {
                return LiquidExpressionResult.Success(liquidExpression.ToOption());
            }
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(liquidExpression.ToOption());
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidCollection)
        {
            if (liquidCollection != null && liquidCollection.Count > 0)
            {
                return LiquidExpressionResult.Success(liquidCollection.ToOption());
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
            Option<ILiquidValue> result = _defaultValue == null
                ? (Option<ILiquidValue>) new None<ILiquidValue>()
                : new Some<ILiquidValue>(_defaultValue);
            return LiquidExpressionResult.Success(result);
        }
    }
}
