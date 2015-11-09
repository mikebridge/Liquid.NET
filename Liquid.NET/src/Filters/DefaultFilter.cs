using System;
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

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return CreateNonDefaultResult(liquidExpression);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidExpression)
        {
            return String.IsNullOrEmpty(liquidExpression.StringVal) ? 
                CreateDefaultResult() : 
                CreateNonDefaultResult(liquidExpression);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidCollection)
        {
            return liquidCollection.Count > 0 ?
                CreateNonDefaultResult(liquidCollection) : 
                CreateDefaultResult();
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return CreateDefaultResult();
        }


        private static LiquidExpressionResult CreateNonDefaultResult(ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(liquidExpression.ToOption());
        }

        private LiquidExpressionResult CreateDefaultResult()
        {
            return LiquidExpressionResult.Success(Option<ILiquidValue>.Create(_defaultValue));
        }
    }
}
