using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Math
{
    public class ModuloFilter: FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public ModuloFilter(NumericValue operand) { _operand = operand; }

        public override NumericValue Apply(NumericValue numericValue)
        {
            if (_operand == null)
            {
                return NumericValue.CreateError("The argument to \"modulo\" is missing.");
            }
            if (_operand.DecimalValue == 0)
            {
                return NumericValue.CreateError("Liquid error: divided by 0");
            }
            return new NumericValue(numericValue.DecimalValue % _operand.DecimalValue);
        }
    }
}
