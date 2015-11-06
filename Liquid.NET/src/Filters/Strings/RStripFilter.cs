using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RStripFilter : FilterExpression<IExpressionConstant, LiquidString>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return  LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => x.TrimEnd()));
        }

    }
}
