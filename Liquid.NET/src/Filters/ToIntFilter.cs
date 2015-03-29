using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;

namespace Liquid.NET.Filters
{
    public class ToIntFilter : FilterExpression<ExpressionConstant, NumericValue>
    {
        public override NumericValue Apply([NotNull] ExpressionConstant liquidExpression)
        {
            return new NumericValue(ToDecimalFilter.ConvertToDecimal(liquidExpression).IntValue);
        }
    }
}
