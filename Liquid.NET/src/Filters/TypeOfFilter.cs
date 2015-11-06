using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TypeOfFilter : FilterExpression<LiquidValue, LiquidString>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(new LiquidString(liquidExpression.LiquidTypeName));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new LiquidString("nil"));
        }

    }
}
