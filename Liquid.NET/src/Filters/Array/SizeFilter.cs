using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override NumericValue Apply(ExpressionConstant objectExpression)
        {
            return ApplyTo((dynamic) objectExpression);
        }

        public IExpressionConstant ApplyTo(IExpressionConstant objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(1); // if it's not an enumerable, it must be of length 1.
        }

        public IExpressionConstant ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.ArrValue.Count);
        }

        public IExpressionConstant ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.StringVal.Length);
        }

        public IExpressionConstant ApplyTo(DictionaryValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(objectExpression.DictValue.Keys.Count);
        }

        public IExpressionConstant ApplyTo(Undefined objectExpression)
        {
            return new NumericValue(0);
        }

        public IExpressionConstant ApplyTo(GeneratorValue objectExpression)
        {
            return new NumericValue(objectExpression.Length);            
        }


    }
}
