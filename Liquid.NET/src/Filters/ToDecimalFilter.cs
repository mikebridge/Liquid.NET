using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToDecimalFilter : FilterExpression<ExpressionConstant, NumericValue>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, ExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(ConvertToDecimal(liquidExpression));
        }

        public static NumericValue ConvertToDecimal(ExpressionConstant liquidExpression)
        {
            
            try
            {
                if (liquidExpression == null || liquidExpression.Value == null)
                {
                    return NumericValue.Create(default(decimal));
                }
                var dec = liquidExpression as NumericValue;
                if (dec != null)
                {
                    return NumericValue.Create(Convert.ToDecimal(dec.DecimalValue));
                }
                var str = liquidExpression as StringValue;
                if (str != null)
                {
                    return NumericValue.Create(Convert.ToDecimal(str.StringVal));
                }
                var bool1 = liquidExpression as BooleanValue;
                if (bool1 != null)
                {
                    return bool1.BoolValue ? NumericValue.Create(1) : NumericValue.Create(0);
                }
            }
            catch (Exception)
            {
                //warningMessage = "unable to convert to a number";
            }
            var result = NumericValue.Create(default(decimal));
//            if (warningMessage != null)
//            {
//                result.WarningMessage = warningMessage;
//            }
            return result;
        }
    }
}
