﻿using System;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Utils
{
    public class LiquidExpressionResult: Either<LiquidError, Option<ILiquidValue>>
    {
        internal LiquidExpressionResult(LiquidError err) : base(err)
        {
        }

        internal LiquidExpressionResult(Option<ILiquidValue> success)
            : base(success)
        {
        }

        public static LiquidExpressionResult ErrorOrNone(ITemplateContext ctx, String varname)
        {
            if (ctx.Options.ErrorWhenValueMissing)
            {
                return Error(SymbolTable.NotFoundError(varname));
            }
            else
            {
                return Success(new None<ILiquidValue>());
            }
        }

        public static LiquidExpressionResult MissingOrNone(ITemplateContext ctx, String varname)
        {
            if (ctx.Options.ErrorWhenVariableMissing)
            {
                return Error(SymbolTable.NotFoundError(varname));
            }
            else
            {
                return Success(new None<ILiquidValue>());
            }
        }

        public LiquidExpressionResult WhenError(Action<LiquidError> fn)
        {
            if (IsLeft)
            {
                fn(Left);
            }
            return this;
        }
        public LiquidExpressionResult WhenSuccess(Action<Option<ILiquidValue>> fn)
        {
            if (IsRight)
            {
                fn(Right);
            }
            return this;
        }

        public Option<ILiquidValue> SuccessResult
        {
            get { return Right; }
        }

        public T SuccessValue<T>()
            where T: ILiquidValue
        {
            return (T) Right.Value;
        }

        public Option<T> SuccessOption<T>()
            where T : ILiquidValue
        {
            return (Option<T>)((object)Right);
        }

        public LiquidError ErrorResult
        {
            get { return Left; }
        }

        public bool IsError { get { return IsLeft; } }

        public bool IsSuccess { get { return IsRight; } }


        public static LiquidExpressionResult Error(String msg)
        {
            return new LiquidExpressionResult(new LiquidError { Message = msg });
        }

        public static LiquidExpressionResult Success(Option<ILiquidValue> success)
        {
            return new LiquidExpressionResult(success);
        }

        public static LiquidExpressionResult Success(String successString)
        {
            return Success(LiquidString.Create(successString));
        }

    }

    public static class LiquidExpressionResultExtensions
    {

        public static LiquidExpressionResult Bind<T>(this LiquidExpressionResult self, Func<Option<T>, LiquidExpressionResult> f)
            where T: ILiquidValue
        {
            if (f == null) throw new ArgumentNullException("f");

            //var result = f(self.SuccessOption<T>());

            //return result; // er, is this whole bind necessary?

            return self.IsError
                ? self
                : f(self.SuccessOption<T>());
                
        }
    }

}
