using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Math
{
    public class TimesFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _addend1;

        public TimesFilter(NumericValue addend1)
        {
            _addend1 = addend1;
        }


        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, NumericValue addend2)
        {
            var result = _addend1.DecimalValue * addend2.DecimalValue;
            return MathHelper.GetReturnValue(result, _addend1, addend2);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(NumericValue.Create(0)); 
        }
    }
}
