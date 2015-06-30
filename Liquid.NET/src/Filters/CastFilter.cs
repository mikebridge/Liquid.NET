using System;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class CastFilter<TSource, TResult> : FilterExpression<TSource, TResult>
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {

        public override LiquidExpressionResult Apply(TSource liquidExpression)
        {
            //Console.WriteLine("Casting from " + typeof(TSource) + " to " + typeof(TResult));
            var result= ValueCaster.Cast<TSource, TResult>(liquidExpression);
            if (result.IsError)
            {
                return result;
            }
            else
            {
                return LiquidExpressionResult.Success(result.SuccessResult);    
            }
            //Console.WriteLine("RESULT IS " + result);
            
        }
    }
}
