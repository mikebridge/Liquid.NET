using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class SizeFilter : FilterExpression<ExpressionConstant, NumericValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            //Console.WriteLine("In IExpressionConstant");
            if (liquidExpression == null || liquidExpression.Value == null)
            {
                return LiquidExpressionResult.Success(new NumericValue(0));
            }
            return LiquidExpressionResult.Success(new NumericValue(1)); // if it's not an enumerable, it must be of length 1.
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Success(new NumericValue(0));
            }
            return LiquidExpressionResult.Success(new NumericValue(liquidArrayExpression.ArrValue.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || liquidStringExpression.Value == null)
            {
                return LiquidExpressionResult.Success(new NumericValue(0));
            }
            return LiquidExpressionResult.Success(new NumericValue(liquidStringExpression.StringVal.Length));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue liquidDictionaryExpression)
        {
            if (liquidDictionaryExpression == null || liquidDictionaryExpression.Value == null)
            {
                return LiquidExpressionResult.Success(new NumericValue(0));
            }
            return LiquidExpressionResult.Success(new NumericValue(liquidDictionaryExpression.DictValue.Keys.Count));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, GeneratorValue liquidGeneratorExpression)
        {
            return LiquidExpressionResult.Success(new NumericValue(liquidGeneratorExpression.Length)); 
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new NumericValue(0)); 
        }
    }
}
