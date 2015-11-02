using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public static class FilterChain
    {

        /// <summary>
        /// Create a chain of functions that returns a value of type resultType.  If function #1 doesn't
        /// return a T, a casting function will be interpolated into the chain.
        /// </summary>
        /// <returns></returns>
        public static Func<Option<IExpressionConstant>, LiquidExpressionResult> CreateChain(
            Type objExprType,
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            var expressions = filterExpressions.ToList();
            if (!expressions.Any())
            {
                return x => new LiquidExpressionResult(x);
            }

            // create the casting filter which will cast the incoming object to the input type of filter #1
            
            Func<IExpressionConstant, LiquidExpressionResult> castFn = 
                objExpr => objExpr!=null ? 
                    CreateCastFilter(objExpr.GetType(), expressions[0].SourceType).Apply(ctx, objExpr) : 
                    LiquidExpressionResult.Success(new None<IExpressionConstant>());
     
            return optionExpression => (castFn(optionExpression.HasValue ? optionExpression.Value : null)).Bind(
                CreateChain(ctx, expressions));

        }


        public static Func<Option<IExpressionConstant>, LiquidExpressionResult> CreateChain(
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return x => BindAll(ctx, InterpolateCastFilters(filterExpressions))(x);
        }

        private static Func<Option<IExpressionConstant>, LiquidExpressionResult> BindAll(
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return initValue => filterExpressions.Aggregate(
                LiquidExpressionResult.Success(initValue), 
                (current, filter) => filter.BindFilter(ctx, current));
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
                    //Console.WriteLine("Creating cast from " + filterExpression +" TO "+expectedInputType);
                    result.Add(CreateCastFilter(expectedInputType, filterExpression.SourceType));
                }
                result.Add(filterExpression);
                expectedInputType = filterExpression.ResultType;
            }

            return result;
        }

        private static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
            //where sourceType: IExpressionConstant
        {
            // TODO: Move this to FilterFactory.Instantiate
            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("FilterChain Creating Converter from " + sourceType + " to " + resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }

    }
}
