using System;

using Liquid.NET.Constants;

namespace Liquid.NET.Utils
{
    public class LiquidExpressionResult<T>: Either<LiquidError, Option<T>> where T: IExpressionConstant
    {
        internal LiquidExpressionResult(LiquidError err) : base(err)
        {
        }

        internal LiquidExpressionResult(Option<T> success)
            : base(success)
        {
        }

        public LiquidExpressionResult<T> WhenError(Action<LiquidError> fn)
        {
            if (IsLeft)
            {
                fn(Left);
            }
            return this;
        }
        public LiquidExpressionResult<T> WhenSuccess(Action<Option<T>> fn)
        {
            if (IsRight)
            {
                fn(Right);
            }
            return this;
        }

        public Option<T> SuccessResult
        {
            get { return Right; }
        }

        public LiquidError ErrorResult
        {
            get { return Left; }
        }

        public bool IsError { get { return IsLeft; } }

        public bool IsSuccess { get { return IsRight; } }


        public static LiquidExpressionResult<T> Error(String msg)
        {
            return new LiquidExpressionResult<T>(new LiquidError { Message = msg });
        }

        public static LiquidExpressionResult<T> Success(Option<T> success)
        {
            return new LiquidExpressionResult<T>(success);
        }

        public static LiquidExpressionResult<T> Success(T success)
        {
            return Success(new Some<T>(success));
        }
    }
}
