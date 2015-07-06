using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class RemoveFilter : FilterExpression<StringValue, StringValue>
    {
        private readonly StringValue _replacement;

        public RemoveFilter(StringValue replacement)
        {
            _replacement = replacement;
        }
        // TODO: use StringUtils.Eval?
        public override LiquidExpressionResult Apply(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            // TODO: Return errors
            Console.WriteLine("APPLYING REMOVE " + _replacement.Value + "TO " + liquidStringExpression.Value);
            if (_replacement == null || _replacement.Value == null)
            {
                return LiquidExpressionResult.Error("Please specify a replacement string.");
            }

            return LiquidExpressionResult.Success(Remove((String)liquidStringExpression.Value, _replacement));

        }

        private StringValue Remove(String orig, StringValue stringToRemove)
        {
            var result = orig.Replace((String) stringToRemove.Value, "");
            Console.WriteLine("  REMOVE RESULT : "+result);
            return new StringValue(result);
        }

    }
}
