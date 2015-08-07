using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class CeilFilter : FilterExpression<NumericValue, NumericValue>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue val)
        {
            var ceiling = (int) System.Math.Ceiling(val.DecimalValue);

            return LiquidExpressionResult.Success(new NumericValue(ceiling));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new NumericValue(0));
        }
    }
}
