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

        public override IExpressionConstant Apply(ExpressionConstant objectExpression)
        {
            return ApplyTo((dynamic)objectExpression);


        }

        public IExpressionConstant ApplyTo(IExpressionConstant objectExpression)
        {
            return ExpressionConstant.CreateError<ArrayValue>("Can't ask for an element at that.  This is not an array or a string.");

        }

        public IExpressionConstant ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return ExpressionConstant.CreateError<ArrayValue>("Array is nil");
            }
            Console.WriteLine("Array is " + objectExpression.ArrValue.Count);
            Console.WriteLine("Index is " + _index.IntValue);
            return objectExpression.ArrValue.Count > 0 && 
                objectExpression.ArrValue.Count >= _index.IntValue + 1 ? 
                objectExpression.ValueAt(_index.IntValue) : 
                ExpressionConstant.CreateError<ArrayValue>("Array has no element at position " + _index.IntValue);
        }

        public IExpressionConstant ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return ExpressionConstant.CreateError<StringValue>("String is nil");
            }
            return objectExpression.StringVal.Length >= _index.IntValue + 1
                ? (IExpressionConstant) new StringValue(objectExpression.StringVal[_index.IntValue].ToString())
                : ExpressionConstant.CreateError<ArrayValue>("String has no element at position " + _index.IntValue);
        }
    }
}
