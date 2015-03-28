using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UniqFilter : FilterExpression<ArrayValue, ArrayValue>
    {

        public override ArrayValue ApplyTo(ArrayValue objectExpression)
        {
            return new ArrayValue(objectExpression.ArrValue.Distinct(new EasyValueComparer()).ToList());
            
        }

    }
}
