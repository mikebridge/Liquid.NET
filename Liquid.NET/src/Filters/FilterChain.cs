using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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
            IEnumerable<IFilterExpression> filterExpressions)
        {
            var expressions = filterExpressions.ToList();
            if (!expressions.Any())
            {
                return x => new LiquidExpressionResult(x);
            }

            // create the casting filter which will cast the incoming object to the input type of filter #1
            
            Func<IExpressionConstant, LiquidExpressionResult> castFn = 
                objExpr => objExpr!=null? CreateCastFilter(objExpr.GetType(), expressions[0].SourceType).Apply(objExpr) : LiquidExpressionResult.Success(new None<IExpressionConstant>());
            //Func<Option<IExpressionConstant>, LiquidExpressionResult> castFn = 
                //objExpr => CreateCastFilter(objExpr.GetType(), expressions[0].SourceType).Apply(objExpr);

            // put the casting filter in between the object and the chain
//            return objExpression => objExpression.Bind(x => castFn(objExpression))
//                                                 .Bind(CreateChain(expressions));      
            // TODO: Figure out how to do this.  It should call ApplyNil() or something.
            return optionExpression => (castFn(optionExpression.HasValue ? optionExpression.Value : null)).Bind(CreateChain(expressions));
            //return optionExpression => optionExpression.HasValue ? castFn(optionExpression.Value) : LiquidExpressionResult.Success(new None<IExpressionConstant>()).Bind(CreateChain(expressions));

            //return objExpression => objExpression.Bind(x => castFn(objExpression))
            //    .Bind(CreateChain(expressions));    

        }


        public static Func<Option<IExpressionConstant>, LiquidExpressionResult> CreateChain(IEnumerable<IFilterExpression> filterExpressions)
        {
            return x =>
            {
                var bindAll = BindAll(InterpolateCastFilters(filterExpressions));
                return bindAll(LiquidExpressionResult.Success(x)); // TODO: Is this the best way to kick off the chain?
            };
        }

        public static Func<LiquidExpressionResult, LiquidExpressionResult> BindAll(
            IEnumerable<IFilterExpression> filterExpressions)
        {
            return initValue => filterExpressions.Aggregate(initValue, (current, filter) =>
            {
                if (initValue.IsError)
                {
                    return initValue;
                }
                LiquidExpressionResult result;
                if (current.SuccessResult.HasValue)
                {
                    result = filter.Apply(current.SuccessResult.Value);
                }
                else
                {
                    result = filter.ApplyToNil();
                }
                return result;
//                if (result.IsSuccess)
//                {
//                    return result.SuccessResult;
//                }
//                else
//                {
//                    //return new LiquidExpressionResult(result.ErrorResult));
//                    //return current;                    
//                    //return null;
//                    //return result.ErrorResult;
//                    return null;
//                }
                
            });
                //LiquidExpressionResult.Error("Error");
                // current.Bind(expression.Apply));
                //return current.Bind(expression.Apply));
                //return current.Bind(expression.Apply)
//                if (current.HasValue)
//                {
//                    return expression.Apply(current.Value)
//                    //var result=expression.Apply(current.Value);
//                    //return result.Bind(result.IsSuccess ? x => expression.Apply(x) : x => result.ErrorResult);
//                    //return current.Bind(result.IsSuccess ? result.SuccessResult : 
//                }
//                else
//                {
//                    var result=expression.ApplyToNil();
//                    return result.IsSuccess ? result.SuccessResult :                     
//                }

            //throw new ApplicationException("Need to figure out how to redo this.");
            //return exprConstant => filterExpressions.Aggregate(exprConstant, (current, expression) => current.Bind(expression.Apply));
//            return exprConstant => filterExpressions.Aggregate(exprConstant, (current, expression) => 
//                current.HasValue ? 
//                    current.Bind(x => expression.Apply(x)) : 
//                    current.Bind(x => expression.ApplyNil()));
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

        public static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
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
