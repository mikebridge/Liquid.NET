using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UpCaseFilter : FilterExpression<LiquidString, LiquidString>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidExpression) 
        {
            return LiquidExpressionResult.Success(liquidExpression.ToString().ToUpper());
        }
    }
}
