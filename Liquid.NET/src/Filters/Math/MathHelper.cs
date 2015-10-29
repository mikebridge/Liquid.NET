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
    public class MathHelper
    {
        public static LiquidExpressionResult GetReturnValue(decimal result, NumericValue val1, NumericValue val2)
        {

            if (val1.IsInt && val2.IsInt)
            {
                //var int32 = Convert.ToInt32(val);
                var int32 = (int)System.Math.Floor(result); // ruby liquid seems to round down.
                return LiquidExpressionResult.Success(NumericValue.Create(int32));
            }
            else
            {
                return LiquidExpressionResult.Success(NumericValue.Create(result));
            }
        }
    }
}
