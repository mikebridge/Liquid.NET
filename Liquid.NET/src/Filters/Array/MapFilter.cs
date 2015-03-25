using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array 
{
    public class MapFilter : FilterExpression<ArrayValue, ArrayValue>
    {
        private readonly StringValue _selector;

        public MapFilter(StringValue selector)
        {
            _selector = selector;
        }

        public override ArrayValue Apply(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return ExpressionConstant.CreateError<ArrayValue>("Array is nil");
            }
            var list = objectExpression.ArrValue.Select(x => TryField(x, _selector.StringVal)).ToList();
            return new ArrayValue(list);
        }

        private IExpressionConstant TryField(IExpressionConstant expressionConstant, string stringVal)
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
