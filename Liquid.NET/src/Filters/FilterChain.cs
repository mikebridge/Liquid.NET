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
        public static Func<Option<ILiquidValue>, LiquidExpressionResult> CreateChain(
            Type objExprType,
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            var expressions = filterExpressions.ToList();
            if (!expressions.Any())
            {
                return x => new LiquidExpressionResult(x);
            }
            // create the initial cast for the first value, then a way of unwrapping the value into an Option,
            // then chain with all the other functions.
            return optionExpression =>
                {
                    var result = CreateUnwrapFunction(InitialCast(ctx, expressions.FirstOrDefault()), optionExpression).Bind(CreateChain(ctx, expressions));
                    return result;
                };

        }

        private static Func<ILiquidValue, LiquidExpressionResult> InitialCast(
            ITemplateContext ctx, IFilterExpression initialExpression)
        {
            return expressionConstant =>
            {
                var result= expressionConstant != null
                    ? CreateCastFilter(expressionConstant.GetType(), initialExpression.SourceType)
                        .Apply(ctx, expressionConstant)
                    : LiquidExpressionResult.Success(new None<ILiquidValue>());
                return result;
            };
        }

        /// <summary>
        /// create the casting filter which will cast the incoming object to the input type of filter #1
        /// </summary>
        /// <param name="castFn"></param>
        /// <param name="optionExpression"></param>
        /// <returns></returns>
        private static LiquidExpressionResult CreateUnwrapFunction(
            Func<ILiquidValue, LiquidExpressionResult> castFn, 
            Option<ILiquidValue> optionExpression)
        {
            return castFn(optionExpression.HasValue ? optionExpression.Value : null);
        }

        public static Func<Option<ILiquidValue>, LiquidExpressionResult> CreateChain(
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return x =>
            {
                var result = BindAll(ctx, InterpolateCastFilters(filterExpressions))(x);
                return result;
            };
        }

        private static Func<Option<ILiquidValue>, LiquidExpressionResult> BindAll(
            ITemplateContext ctx,
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return initValue => filterExpressions.Aggregate(
                LiquidExpressionResult.Success(initValue),
                (current, filter) =>
                {
                    var result = filter.BindFilter(ctx, current);
                    return result;
                });
        }

        /// <summary>
        /// Make a list of functions, each of which has the input of the previous function.  Interpolate a casting
        /// function if the input of one doesn't fit with the value of the next.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IFilterExpression> InterpolateCastFilters(IEnumerable<IFilterExpression> filterExpressions)
        {

            var result = new List<IFilterExpression>();

            Type expectedInputType = null;
            foreach (var filterExpression in filterExpressions)
            {
                if (NeedsACast(expectedInputType, filterExpression))
                {
                    result.Add(CreateCastFilter(expectedInputType, filterExpression.SourceType));
                }
                result.Add(filterExpression);
                expectedInputType = filterExpression.ResultType;
            }

            return result;
        }

        private static bool NeedsACast(Type expectedInputType, IFilterExpression filterExpression)
        {
            return expectedInputType != null && !filterExpression.SourceType.IsAssignableFrom(expectedInputType);
        }

        private static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
        {
            Type genericClass = typeof(CastFilter<,>);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }

    }
}
