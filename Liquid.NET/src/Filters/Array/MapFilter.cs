using System;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array 
{
    public class MapFilter: FilterExpression<IExpressionConstant , IExpressionConstant>// : FilterExpression<ArrayValue, ArrayValue>
    {
        private readonly StringValue _selector;

        public MapFilter(StringValue selector)
        {
            _selector = selector;
        }

        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression)
        {
            //return LiquidExpressionResult.Error("Can't map that object type.  It is not an array or a hash.");
            return LiquidExpressionResult.Success(new None<IExpressionConstant>());
        }

        public override LiquidExpressionResult ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            var list = liquidArrayExpression.ArrValue.Select(x => x.HasValue ? 
                FieldAccessor.TryField(x.Value, _selector.StringVal) : 
                new None<IExpressionConstant>()).ToList();
            return LiquidExpressionResult.Success(new ArrayValue(list));
        }

        public override LiquidExpressionResult ApplyTo(DictionaryValue liquidDictionaryValue)
        {
            if (liquidDictionaryValue == null || liquidDictionaryValue.Value == null)
            {
                return LiquidExpressionResult.Error("Hash is nil");
            }
            String propertyNameString = ValueCaster.RenderAsString((IExpressionConstant)_selector);
             return LiquidExpressionResult.Success(liquidDictionaryValue.ValueAt(propertyNameString));
            
        }

    }
}
