using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RemoveFirstFilter : FilterExpression<IExpressionConstant, LiquidString>
    {
        private readonly LiquidString _liquidStringToRemove;

        public RemoveFirstFilter(LiquidString liquidStringToRemove)
        {
            _liquidStringToRemove = liquidStringToRemove;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => StringUtils.ReplaceFirst(x, _liquidStringToRemove.StringVal, "")));            
        }

       
    }
}
