using System;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToDecimalFilter : FilterExpression<LiquidValue, LiquidNumeric>
    {
        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Success(ConvertToDecimal(liquidExpression));
        }

        public static LiquidNumeric ConvertToDecimal(LiquidValue liquidExpression)
        {
            
            try
            {
                if (liquidExpression == null || liquidExpression.Value == null)
                {
                    return LiquidNumeric.Create(default(decimal));
                }
                var dec = liquidExpression as LiquidNumeric;
                if (dec != null)
                {
                    return LiquidNumeric.Create(Convert.ToDecimal(dec.DecimalValue));
                }
                var str = liquidExpression as LiquidString;
                if (str != null)
                {
                    return LiquidNumeric.Create(Convert.ToDecimal(str.StringVal));
                }
                var bool1 = liquidExpression as LiquidBoolean;
                if (bool1 != null)
                {
                    return bool1.BoolValue ? LiquidNumeric.Create(1) : LiquidNumeric.Create(0);
                }
            }
            catch (Exception)
            {
                //warningMessage = "unable to convert to a number";
            }
            var result = LiquidNumeric.Create(default(decimal));
//            if (warningMessage != null)
//            {
//                result.WarningMessage = warningMessage;
//            }
            return result;
        }
    }
}
