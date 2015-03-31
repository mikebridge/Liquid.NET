using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;

namespace Liquid.NET.Filters.Math
{
    public class TimesFilter : FilterExpression<NumericValue, NumericValue>
    {
        private readonly NumericValue _addend1;

        public TimesFilter(NumericValue addend1)
        {
            _addend1 = addend1;
        }


        public override NumericValue ApplyTo(NumericValue addend2)
        {
            return new NumericValue(_addend1.DecimalValue * addend2.DecimalValue);
        }

    }
}
