using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class RemoveFilter : FilterExpression<LiquidString, LiquidString>
    {
        private readonly LiquidString _replacement;

        public RemoveFilter(LiquidString replacement)
        {
            _replacement = replacement;
        }
        // TODO: use StringUtils.Eval?
        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            // TODO: Return errors
            //Console.WriteLine("APPLYING REMOVE " + _replacement.Value + "TO " + liquidLiquidStringExpression.Value);
            if (_replacement == null || _replacement.Value == null)
            {
                return LiquidExpressionResult.Error("Please specify a replacement string.");
            }

            return LiquidExpressionResult.Success(Remove((String)liquidLiquidStringExpression.Value, _replacement));

        }

        private LiquidString Remove(String orig, LiquidString liquidStringToRemove)
        {
            var result = orig.Replace((String) liquidStringToRemove.Value, "");
            //Console.WriteLine("  REMOVE RESULT : "+result);
            return LiquidString.Create(result);
        }

    }
}
