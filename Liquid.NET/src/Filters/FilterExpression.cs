using System;

using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public interface IFilterExpression
    {
        String Name { get; set; }

        Type SourceType { get; }
        Type ResultType { get; }

        LiquidExpressionResult Apply(ITemplateContext ctx, IExpressionConstant liquidExpression);
        
        LiquidExpressionResult ApplyToNil(ITemplateContext ctx);

    };

    public interface IFilterExpression<in TSource, out TResult> : IFilterExpression
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {
        TResult Apply(ITemplateContext ctx, TSource liquidExpression);
    };

    public abstract class FilterExpression<TSource, TResult> : IFilterExpression
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {
        public String Name {get; set;}


        public virtual LiquidExpressionResult Apply(ITemplateContext ctx, TSource option)
        {
            return ApplyTo(ctx, (dynamic)option);
        }

        public LiquidExpressionResult Apply(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return Apply(ctx, (TSource)liquidExpression);
        }

   
        /* override some or all of these ApplyTo functions */
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression) // todo: make this fallback abtstract
        {
            throw new NotImplementedException();
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, NumericValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx,  (IExpressionConstant)val);
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, BooleanValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, DateValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }


        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, GeneratorValue val)
        {
            //throw new Exception("Need to figure out how to do this part");
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new None<IExpressionConstant>());
            //return new LiquidExpressionResult(Option<IExpressionConstant>.None);
        }

        public Type SourceType
        {
            get { return typeof(TSource); }
        }

        public Type ResultType
        {
            get { return typeof(TResult); }
        }

    }
}