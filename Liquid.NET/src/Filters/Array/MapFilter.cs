using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array 
{
    public class MapFilter: FilterExpression<IExpressionConstant , IExpressionConstant>// : FilterExpression<ArrayValue, ArrayValue>
    {
        private readonly StringValue _selector;

        public MapFilter(StringValue selector)
        {
            _selector = selector;
        }

        public override IExpressionConstant ApplyTo(IExpressionConstant liquidExpression)
        {
            return ConstantFactory.CreateError<ArrayValue>("Can't map that object type.  It is not an array or a hash.");
        }

        public override IExpressionConstant ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            var list = liquidArrayExpression.ArrValue.Select(x => FieldAccessor.TryField(x, _selector.StringVal)).ToList();
            return new ArrayValue(list);
        }

        public override IExpressionConstant ApplyTo(DictionaryValue liquidDictionaryValue)
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
