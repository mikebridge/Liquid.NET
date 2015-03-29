using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class PositionFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly NumericValue _index;

        public PositionFilter(NumericValue index)
        {
            // TODO: Document what happens if htere's no arg passed.  Is this null?  What do we do?
            _index = index;
        }

        public override IExpressionConstant ApplyTo(IExpressionConstant liquidExpression)
        {
            return ConstantFactory.CreateError<ArrayValue>("Can't find sub-elements from that object.  It is not an array or a string.");
        }

        public override IExpressionConstant ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            Console.WriteLine("Array is " + liquidArrayExpression.ArrValue.Count);
            Console.WriteLine("Index is " + _index.IntValue);
            return liquidArrayExpression.ArrValue.Count > 0 && 
                liquidArrayExpression.ArrValue.Count >= _index.IntValue + 1 ? 
                liquidArrayExpression.ValueAt(_index.IntValue) : 
                ConstantFactory.CreateError<ArrayValue>("Array has no element at position " + _index.IntValue);
        }

        public override IExpressionConstant ApplyTo(StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || liquidStringExpression.Value == null)
            {
                return ConstantFactory.CreateError<StringValue>("String is nil");
            }
            return liquidStringExpression.StringVal.Length >= _index.IntValue + 1
                ? (IExpressionConstant)new StringValue(liquidStringExpression.StringVal[_index.IntValue].ToString())
                : ConstantFactory.CreateError<ArrayValue>("String has no element at position " + _index.IntValue);
        }
    }
}
