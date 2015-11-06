using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReplaceFirstFilter : FilterExpression<ILiquidValue, LiquidString>
    {
        private readonly LiquidString _liquidStringToRemove;
        private readonly LiquidString _replacementLiquidString;

        public ReplaceFirstFilter(LiquidString liquidStringToRemove, LiquidString replacementLiquidString)
        {
            _liquidStringToRemove = liquidStringToRemove ?? new LiquidString(""); 
            _replacementLiquidString = replacementLiquidString ?? new LiquidString("");
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => StringUtils.ReplaceFirst(x, _liquidStringToRemove.StringVal, _replacementLiquidString.StringVal)));
        }


    }
}
