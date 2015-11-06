using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class FieldAccessor
    {
        public static LiquidExpressionResult TryField(
            ITemplateContext ctx, 
            IExpressionConstant expressionConstant, 
            string index)
        {

            var dict = expressionConstant as DictionaryValue;
            if (dict == null)
            {
                return LiquidExpressionResult.ErrorOrNone(ctx, index);
                          
            }

            return dict.ContainsKey(index)
                ? LiquidExpressionResult.Success(dict[index])
                : LiquidExpressionResult.ErrorOrNone(ctx, index);
        }
    }
}
