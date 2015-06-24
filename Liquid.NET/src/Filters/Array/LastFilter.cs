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
        public override LiquidExpressionResult Apply(ExpressionConstant liquidExpression)
        {
            return ApplyTo((dynamic)liquidExpression);


        }

        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Error("Can't ask for an element at that index.  This is not an array or a string.");

        }

        public override LiquidExpressionResult ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(liquidArrayExpression.ArrValue.Count - 1));
            return positionFilter.ApplyTo(liquidArrayExpression);
        }

        public override LiquidExpressionResult ApplyTo(StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || String.IsNullOrEmpty(liquidStringExpression.StringVal))
            {
                return LiquidExpressionResult.Error("String is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(liquidStringExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(liquidStringExpression);
        }
    }
}
