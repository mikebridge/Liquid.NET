using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public static class StringResult
    {
        public static StringValue Eval(IExpressionConstant objectExpression, Func<String, String> f)
        {
            String before = ValueCaster.RenderAsString(objectExpression);

            if (String.IsNullOrWhiteSpace(before))
            {
                return new StringValue("");
            }
            return new StringValue(f(before));
        }
    }
}
