using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class ToDecimalFilter : FilterExpression<ExpressionConstant, NumericValue>
    {
        public override LiquidExpressionResult Apply(ExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(ConvertToDecimal(liquidExpression));
        }

        public static NumericValue ConvertToDecimal(ExpressionConstant liquidExpression)
        {
            String warningMessage = null;
            try
            {
                if (liquidExpression == null || liquidExpression.Value == null)
                {
                    return new NumericValue(default(decimal));
                }
                var dec = liquidExpression as NumericValue;
                if (dec != null)
                {
                    return new NumericValue(Convert.ToDecimal(dec.DecimalValue));
                }
                var str = liquidExpression as StringValue;
                if (str != null)
                {
                    return new NumericValue(Convert.ToDecimal(str.StringVal));
                }
                var bool1 = liquidExpression as BooleanValue;
                if (bool1 != null)
                {
                    return bool1.BoolValue ? new NumericValue(1) : new NumericValue(0);
                }
            }
            catch (Exception)
            {
                //warningMessage = "unable to convert to a number";
            }
            var result = new NumericValue(default(decimal));
//            if (warningMessage != null)
//            {
//                result.WarningMessage = warningMessage;
//            }
            return result;
        }
    }
}
