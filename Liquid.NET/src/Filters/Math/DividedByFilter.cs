using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DividedByFilter : FilterExpression<NumericValue, NumericValue>
    {        
        private readonly NumericValue _divisor;

        public DividedByFilter(NumericValue divisor)
        {
            _divisor = divisor;
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, NumericValue dividend)
        {
            if (dividend == null)
            {
                return LiquidExpressionResult.Error("The dividend is missing.");
            }
            if (_divisor == null)
            {
                return LiquidExpressionResult.Error("The divisor is missing.");
            }
            if (_divisor.DecimalValue == 0.0m)
            {
                return LiquidExpressionResult.Error("Liquid error: divided by 0");
            }
            var result = dividend.DecimalValue / _divisor.DecimalValue;
            return MathHelper.GetReturnValue(result, dividend, _divisor);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return Apply(ctx, NumericValue.Create(0));

        }
    }

}
