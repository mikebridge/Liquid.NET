using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class FieldAccessor
    {
        public static Option<IExpressionConstant> TryField(IExpressionConstant expressionConstant, string stringVal)
        {
            var dict = expressionConstant as DictionaryValue;
            if (dict == null)
            {
                return new None<IExpressionConstant>();
               
            }
            return dict.DictValue.ContainsKey(stringVal) ? dict.DictValue[stringVal] : new None<IExpressionConstant>();
        }
    }
}
