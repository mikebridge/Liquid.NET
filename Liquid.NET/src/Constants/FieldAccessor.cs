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

            return dict.DictValue.ContainsKey(index)
                ? LiquidExpressionResult.Success(dict.DictValue[index])
                : LiquidExpressionResult.ErrorOrNone(ctx, index);
        }
    }
}
