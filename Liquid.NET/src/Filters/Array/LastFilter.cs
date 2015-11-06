using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class LastFilter : FilterExpression<LiquidValue, IExpressionConstant>
    {
        /// <summary>
        ///  TODO: Update to new structure
        /// </summary>
        /// <returns></returns>
        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidValue liquidExpression)
        {
            //return ApplyTo(ctx, (dynamic)liquidExpression);
            var arr = liquidExpression as LiquidCollection;
            if (arr != null)
            {
                return ApplyTo(ctx, arr);
            }

            var str = liquidExpression as LiquidString;
            if (str != null)
            {
                return ApplyTo(ctx, str);
            }
            return ApplyTo(ctx, liquidExpression);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Error("Can't ask for an element at that index.  This is not an array or a string.");

        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            var positionFilter = new PositionFilter(LiquidNumeric.Create(liquidArrayExpression.Count - 1));
            return positionFilter.ApplyTo(ctx, liquidArrayExpression);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            if (liquidLiquidStringExpression == null || String.IsNullOrEmpty(liquidLiquidStringExpression.StringVal))
            {
                return LiquidExpressionResult.Error("String is nil");
            }
            var positionFilter = new PositionFilter(LiquidNumeric.Create(liquidLiquidStringExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(ctx, liquidLiquidStringExpression);
        }
    }
}
