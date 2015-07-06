using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToIntFilter : FilterExpression<ExpressionConstant, NumericValue>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, ExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(new NumericValue(ToDecimalFilter.ConvertToDecimal(liquidExpression).IntValue));
        }
    }
}
