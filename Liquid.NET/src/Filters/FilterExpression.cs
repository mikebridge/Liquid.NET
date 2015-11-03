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

        LiquidExpressionResult BindFilter(ITemplateContext ctx, LiquidExpressionResult current);
    };

    public abstract class FilterExpression: IFilterExpression
    {
        public virtual String Name { get; set; }
        public abstract Type SourceType { get; }
        public abstract Type ResultType { get; }
        public abstract LiquidExpressionResult Apply(ITemplateContext ctx, IExpressionConstant liquidExpression);
        

        public LiquidExpressionResult BindFilter(ITemplateContext ctx, LiquidExpressionResult current)
        {
            return current.IsError ? current : ApplyValueOrNil(ctx, current);
        }

        private LiquidExpressionResult ApplyValueOrNil(ITemplateContext ctx, LiquidExpressionResult current)
        {
            return current.SuccessResult.HasValue
                ? Apply(ctx, current.SuccessResult.Value)
                : ApplyToNil(ctx); // pass through nil, because maybe there's e.g. a "default" filter somewhere in the chain.
        }

        /* override some or all of these ApplyTo functions */
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext _, IExpressionConstant liquidExpression)
        {
            throw new NotImplementedException();
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, NumericValue val)
        {
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue val)
        {
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue val)
        {
            return ApplyTo(ctx, (IExpressionConstant)val);
        }
        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue val)
        {
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, BooleanValue val)
        {
            return ApplyTo(ctx, (IExpressionConstant)val);
        }

        public virtual LiquidExpressionResult ApplyTo(ITemplateContext ctx, DateValue val)
        {
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

    }

    public abstract class FilterExpression<TSource, TResult> : FilterExpression
        where TSource : IExpressionConstant
        where TResult : IExpressionConstant
    {


        public virtual LiquidExpressionResult Apply(ITemplateContext ctx, TSource expr)
        {
            //return ApplyTo(ctx, (dynamic)expr);
            var str = expr as StringValue;
            if (str!=null)
            {
                return ApplyTo(ctx, str);
            }
            var num = expr as NumericValue;
            if (num != null)
            {
                return ApplyTo(ctx, num);
            }
            var boo = expr as BooleanValue;
            if (boo != null)
            {
                return ApplyTo(ctx, boo);
            }
            var dict = expr as DictionaryValue;
            if (dict != null)
            {
                return ApplyTo(ctx, dict);
            }
            var arr = expr as ArrayValue;
            if (arr != null)
            {
                return ApplyTo(ctx, arr);
            }
            var date = expr as DateValue;
            if (date != null)
            {
                return ApplyTo(ctx, date);
            }
            var gen = expr as GeneratorValue;
            if (gen != null)
            {
                return ApplyTo(ctx, gen);
            }
            return ApplyTo(ctx, expr);
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return Apply(ctx, (TSource)liquidExpression);
        }
        

        public override Type SourceType
        {
            get { return typeof(TSource); }
        }

        public override Type ResultType
        {
            get { return typeof(TResult); }
        }

    }
}