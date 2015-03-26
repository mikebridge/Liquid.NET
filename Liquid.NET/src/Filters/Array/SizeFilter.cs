using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override NumericValue Apply(ExpressionConstant objectExpression)
        {
            //Func<StringValue, NumericValue> func = x => ApplyTo((dynamic)x);
            return (NumericValue)objectExpression.Bind(x => ApplyTo(x));
            //return (NumericValue) objectExpression.Bind(x => ApplyTo((dynamic) objectExpression));
            //return ApplyTo((dynamic) objectExpression);
        }

        public NumericValue ApplyTo(IExpressionConstant objectExpression)
        {
            
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(1); // if it's not an enumerable, it must be of length 1.
        }

        public NumericValue ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.ArrValue.Count);
        }

        public NumericValue ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.StringVal.Length);
        }

        public NumericValue ApplyTo(DictionaryValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.DictValue.Keys.Count);
        }

        public NumericValue ApplyTo(GeneratorValue objectExpression)
        {
            return new NumericValue(objectExpression.Length);            
        }


    }
}
