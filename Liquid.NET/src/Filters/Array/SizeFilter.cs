using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override NumericValue ApplyTo(IExpressionConstant liquidExpression)
        {
            Console.WriteLine("In IExpressionConstant");
            if (liquidExpression == null || liquidExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(1); // if it's not an enumerable, it must be of length 1.
        }

        public override NumericValue ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(liquidArrayExpression.ArrValue.Count);
        }

        public override NumericValue ApplyTo(StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || liquidStringExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(liquidStringExpression.StringVal.Length);
        }

        public override NumericValue ApplyTo(DictionaryValue liquidDictionaryExpression)
        {
            if (liquidDictionaryExpression == null || liquidDictionaryExpression.Value == null)
            {
                return new NumericValue(0);
            }
            return new NumericValue(liquidDictionaryExpression.DictValue.Keys.Count);
        }

        public override NumericValue ApplyTo(GeneratorValue liquidGeneratorExpression)
        {
            return new NumericValue(liquidGeneratorExpression.Length);            
        }


    }
}
