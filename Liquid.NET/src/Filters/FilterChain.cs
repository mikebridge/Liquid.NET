using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;

namespace Liquid.NET.Filters
{
    public static class FilterChain
    {

        /// <summary>
        /// Create a chain of functions that returns a value of type resultType.  If function #1 doesn't
        /// return a T, a casting function will be interpolated into the chain.
        /// </summary>
        /// <returns></returns>
        public static Func<IExpressionConstant, IExpressionConstant> CreateChain(
            Type objExprType,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            var expressions = filterExpressions.ToList();
            if (!expressions.Any())
            {
                return x => x;
            }

            // create the casting filter which will cast the incoming object to the input type of filter #1
            Func<IExpressionConstant, IExpressionConstant> castFn = objExpr => CreateCastFilter(objExpr.GetType(), expressions[0].SourceType).Apply(objExpr);

            // put the casting filter in between the object and the chain
            return objExpression => objExpression.Bind(x => castFn(objExpression))
                                                 .Bind(CreateChain(expressions));
           

        }


        public static Func<IExpressionConstant, IExpressionConstant> CreateChain(IEnumerable<IFilterExpression> filterExpressions)
        {
            return x => BindAll(InterpolateCastFilters(filterExpressions))(x);
        }

        public static Func<IExpressionConstant, IExpressionConstant> BindAll(
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return exprConstant => filterExpressions.Aggregate(exprConstant, (current, expression) => current.Bind(expression.Apply));
        }

        /// <summary>
        /// Make a list of functions, each of which has the input of the previous function.  Interpolate a casting
        /// function if the input of one doesn't fit with the value of the next.
        /// 
        /// TODO: I think this should be part of the bind function.
        /// </summary>
        /// <param name="filterExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<IFilterExpression> InterpolateCastFilters(IEnumerable<IFilterExpression> filterExpressions)
        {

            var result = new List<IFilterExpression>();

            Type expectedInputType = null;
            foreach (var filterExpression in filterExpressions)
            {
                // TODO: The expectedInputType might be a superclass of the output (not just equal type)
                //if (expectedInputType != null && filterExpression.SourceType != expectedInputType)
                if (expectedInputType != null && !filterExpression.SourceType.IsAssignableFrom(expectedInputType))
                {
                    Console.WriteLine("Creating cast from " + filterExpression +" TO "+expectedInputType);
                    result.Add(CreateCastFilter(expectedInputType, filterExpression.SourceType));
                }
                result.Add(filterExpression);
                expectedInputType = filterExpression.ResultType;
            }

            return result;
        }

        public static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
        {
            // TODO: Move this to FilterFactory.Instantiate
            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            Console.WriteLine("FilterChain Creating Converter from " + sourceType + " to " + resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }

    }
}
