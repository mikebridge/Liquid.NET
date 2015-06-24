using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class CastFilter<TSource, TResult> : FilterExpression<TSource, TResult>
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {

        public override LiquidExpressionResult Apply(TSource liquidExpression)
        {
            Console.WriteLine("Casting from " + typeof(TSource) + " to " + typeof(TResult));
            var result= ValueCaster.Cast<TSource, TResult>(liquidExpression);
            Console.WriteLine("RESULT IS " + result);
            return result;
        }
    }
}
