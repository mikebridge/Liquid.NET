using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TypeOfFilter : FilterExpression<ExpressionConstant, StringValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(new StringValue(liquidExpression.LiquidTypeName));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new StringValue("nil"));
        }

    }
}
