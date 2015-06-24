using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return LiquidExpressionResult.Error("Can't map that object type.  It is not an array or a hash.");
        }

        public override LiquidExpressionResult ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            var list = liquidArrayExpression.ArrValue.Select(x => FieldAccessor.TryField(x, _selector.StringVal)).ToList();
            return LiquidExpressionResult.Success(new ArrayValue(list));
        }

        public override LiquidExpressionResult ApplyTo(DictionaryValue liquidDictionaryValue)
        {
            if (liquidDictionaryValue == null || liquidDictionaryValue.Value == null)
            {
                return ConstantFactory.CreateError<DictionaryValue>("Hash is nil");
            }
             String propertyNameString = ValueCaster.RenderAsString(_selector);
            return liquidDictionaryValue.ValueAt(propertyNameString);
            
        }

//
//        public MapFilter(StringValue selector)
//        {
//            _selector = selector;
//        }
//
//        public override ArrayValue Apply(ArrayValue liquidArrayExpression)
//        {
//            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
//            {
//                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
//            }
//            var list = liquidArrayExpression.ArrValue.Select(x => FieldAccessor.TryField(x, _selector.StringVal)).ToList();
//            return new ArrayValue(list);
//        }
    }
}
