using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override NumericValue ApplyTo(IExpressionConstant objectExpression)
        {
            Console.WriteLine("In IExpressionConstant");
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(1); // if it's not an enumerable, it must be of length 1.
        }

        public override NumericValue ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.ArrValue.Count);
        }

        public override NumericValue ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.StringVal.Length);
        }

        public override NumericValue ApplyTo(DictionaryValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.DictValue.Keys.Count);
        }

        public override NumericValue ApplyTo(GeneratorValue objectExpression)
        {
            return new NumericValue(objectExpression.Length);            
        }


    }
}
