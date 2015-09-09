using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class LastFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        /// <summary>
        ///  TODO: Update to new structure
        /// </summary>
        /// <returns></returns>
        public override LiquidExpressionResult Apply(ITemplateContext ctx, ExpressionConstant liquidExpression)
        {
            //return ApplyTo(ctx, (dynamic)liquidExpression);
            var arr = liquidExpression as ArrayValue;
            if (arr != null)
            {
                return ApplyTo(ctx, arr);
            }

            var str = liquidExpression as StringValue;
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

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            var positionFilter = new PositionFilter(NumericValue.Create(liquidArrayExpression.ArrValue.Count - 1));
            return positionFilter.ApplyTo(ctx, liquidArrayExpression);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || String.IsNullOrEmpty(liquidStringExpression.StringVal))
            {
                return LiquidExpressionResult.Error("String is nil");
            }
            var positionFilter = new PositionFilter(NumericValue.Create(liquidStringExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(ctx, liquidStringExpression);
        }
    }
}
