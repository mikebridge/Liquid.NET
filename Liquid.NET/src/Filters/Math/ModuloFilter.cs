using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class ModuloFilter: FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _operand;

        public ModuloFilter(NumericValue operand) { _operand = operand; }

        public override LiquidExpressionResult Apply(NumericValue numericValue)
        {
            if (_operand == null)
            {
                return LiquidExpressionResult.Error("The argument to \"modulo\" is missing.");
            }
            if (_operand.DecimalValue == 0)
            {
                return LiquidExpressionResult.Error("Liquid error: divided by 0");
            }
            return LiquidExpressionResult.Success(new NumericValue(numericValue.DecimalValue % _operand.DecimalValue));
        }
    }
}
