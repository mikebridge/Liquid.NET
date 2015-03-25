using System;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;

namespace Liquid.NET.Filters
{
    public interface IFilterExpression
    {
        Type SourceType { get; }
        Type ResultType { get; }

        IExpressionConstant Apply(IExpressionConstant objectExpression);
    };

    public interface IFilterExpression<in TSource, out TResult> : IFilterExpression        
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {
        TResult Apply(TSource objectExpression);
    };

    public abstract class FilterExpression<TSource,TResult> : IFilterExpression
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {

        public abstract TResult Apply(TSource objectExpression);

        public IExpressionConstant Apply(IExpressionConstant objectExpression)
        {
            return Apply((TSource) objectExpression);
        }

        public Type SourceType
        {
            get { return typeof(TSource);  }
        }

        public Type ResultType
        {
            get { return typeof(TResult); }
        }


    }
}
