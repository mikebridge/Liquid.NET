using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReplaceFilter : FilterExpression<ILiquidValue, LiquidString>
    {
        private readonly LiquidString _liquidStringToRemove;
        private readonly LiquidString _replacementLiquidString;

        public ReplaceFilter(LiquidString liquidStringToRemove, LiquidString replacementLiquidString)
        {
            _liquidStringToRemove = liquidStringToRemove;
            _replacementLiquidString = replacementLiquidString;
        }


        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => x.Replace(_liquidStringToRemove.StringVal, _replacementLiquidString.StringVal)));
        }

    }
}
