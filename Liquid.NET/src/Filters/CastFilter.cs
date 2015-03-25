using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;

namespace Liquid.NET.Filters
{
    public class CastFilter<TSource, TResult> :FilterExpression<TSource, TResult>
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {

        public override TResult Apply(TSource objectExpression)
        {
            Console.WriteLine("Casting from " + typeof(TSource) + " to " + typeof(TResult));
            var result= ValueCaster.Cast<TSource, TResult>(objectExpression);
            Console.WriteLine("RESULT IS " + result);
            return result;
        }
    }
}
