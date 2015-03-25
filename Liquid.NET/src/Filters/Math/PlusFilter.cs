using System;
using Antlr4.Runtime.Misc;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    //public class PlusFilter : FilterExpression<NumericLiteral, NumericLiteral>
    public class PlusFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public PlusFilter([NotNull] NumericValue operand)
        {
            Console.WriteLine("Not Null");
            _operand = operand;
        }

        public override NumericValue Apply([NotNull] NumericValue numericValue)
        {
            if (_operand == null)
            {
                return NumericValue.CreateError("The argument to \"add\" is missing.");
            }
            return new NumericValue(numericValue.DecimalValue +  _operand.DecimalValue);
        }
    }
}
