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

        //public abstract TResult Apply(TSource objectExpression);
        public virtual TResult Apply(TSource objectExpression)
        {
            return objectExpression.Bind<TResult>(x => ApplyTo((dynamic)x));
        }

        public IExpressionConstant Apply(IExpressionConstant objectExpression)
        {
            return Apply((TSource)objectExpression);
        }

        /* override some or all of these ApplyTo functions */
        public virtual TResult ApplyTo(IExpressionConstant val) // todo: make this fallback abtstract
        {
            throw new NotImplementedException();
        }
        public virtual TResult ApplyTo(NumericValue val) 
        {
            throw new NotImplementedException();
        }
        public virtual TResult ApplyTo(StringValue val)
        {
            throw new NotImplementedException();
        }
        public virtual TResult ApplyTo(DictionaryValue val)
        {
            throw new NotImplementedException();
        }
        public virtual TResult ApplyTo(ArrayValue val)
        {
            throw new NotImplementedException();
        }

        public virtual TResult ApplyTo(BooleanValue val)
        {
            throw new NotImplementedException();
        }

        public virtual TResult ApplyTo(GeneratorValue val)
        {
            throw new NotImplementedException();
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
