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
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            var list = objectExpression.ArrValue.Select(x => FieldAccessor.TryField(x, _selector.StringVal)).ToList();
            return new ArrayValue(list);
        }
    }
}
