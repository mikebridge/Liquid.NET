using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return GetSize(liquidExpression, () => NumericValue.Create(1));// if it's not an enumerable, it must be of length 1.
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {
            return GetSize(liquidArrayExpression, () => NumericValue.Create(liquidArrayExpression.ArrValue.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            return GetSize(liquidStringExpression, () => NumericValue.Create(liquidStringExpression.StringVal.Length));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue liquidDictionaryExpression)
        {
            return GetSize(liquidDictionaryExpression, () => NumericValue.Create(liquidDictionaryExpression.DictValue.Keys.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, GeneratorValue liquidGeneratorExpression)
        {
            //return GetSize(liquidGeneratorExpression, () => NumericValue.Create(liquidGeneratorExpression.Length));
            return liquidGeneratorExpression == null
                ? SizeOfNil()
                : LiquidExpressionResult.Success(NumericValue.Create(liquidGeneratorExpression.Length));
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return SizeOfNil(); 
        }

        private static LiquidExpressionResult GetSize(IExpressionConstant liquidExpression, Func<NumericValue> measureSizeFunc)
        {
            return liquidExpression == null || liquidExpression.Value == null
                ? SizeOfNil()
                : LiquidExpressionResult.Success(measureSizeFunc());
        }

        private static LiquidExpressionResult SizeOfNil()
        {
            return LiquidExpressionResult.Success(NumericValue.Create(0));
        }
    }
}
