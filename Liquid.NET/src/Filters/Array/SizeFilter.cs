using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<LiquidValue, LiquidNumeric>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return GetSize(liquidExpression, () => LiquidNumeric.Create(1));// if it's not an enumerable, it must be of length 1.
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
        {
            return GetSize(liquidArrayExpression, () => LiquidNumeric.Create(liquidArrayExpression.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            return GetSize(liquidLiquidStringExpression, () => LiquidNumeric.Create(liquidLiquidStringExpression.StringVal.Length));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidHash liquidDictionaryExpression)
        {
            return GetSize(liquidDictionaryExpression, () => LiquidNumeric.Create(liquidDictionaryExpression.Keys.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidRange liquidGeneratorExpression)
        {
            //return GetSize(liquidGeneratorExpression, () => LiquidNumeric.Create(liquidGeneratorExpression.Length));
            return liquidGeneratorExpression == null
                ? SizeOfNil()
                : LiquidExpressionResult.Success(LiquidNumeric.Create(liquidGeneratorExpression.Length));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return SizeOfNil(); 
        }

        private static LiquidExpressionResult GetSize(IExpressionConstant liquidExpression, Func<LiquidNumeric> measureSizeFunc)
        {
            return liquidExpression == null || liquidExpression.Value == null
                ? SizeOfNil()
                : LiquidExpressionResult.Success(measureSizeFunc());
        }

        private static LiquidExpressionResult SizeOfNil()
        {
            return LiquidExpressionResult.Success(LiquidNumeric.Create(0));
        }
    }
}
