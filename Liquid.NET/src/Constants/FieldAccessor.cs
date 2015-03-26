using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Constants
{
    public static class FieldAccessor
    {
        public static IExpressionConstant TryField(IExpressionConstant expressionConstant, string stringVal)
        {
            var dict = expressionConstant as DictionaryValue;
            if (dict == null)
            {
                return new Undefined(stringVal);
            }
            if (!dict.DictValue.ContainsKey(stringVal))
            {
                return new Undefined(stringVal);
            }
            else
            {
                return dict.DictValue[stringVal];
            }
        }
    }
}
