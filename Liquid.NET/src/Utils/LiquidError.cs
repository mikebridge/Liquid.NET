using System;

using Liquid.NET.Constants;

namespace Liquid.NET.Utils
{
    public class LiquidExpressionResult: Either<LiquidError, Option<IExpressionConstant>>
    {
        internal LiquidExpressionResult(LiquidError err) : base(err)
        {
        }

        internal LiquidExpressionResult(Option<IExpressionConstant> success)
            : base(success)
        {
        }

        public LiquidExpressionResult WhenError(Action<LiquidError> fn)
        {
            if (IsLeft)
            {
                fn(Left);
            }
            return this;
        }
        public LiquidExpressionResult WhenSuccess(Action<Option<IExpressionConstant>> fn)
        {
            if (IsRight)
            {
                fn(Right);
            }
            return this;
        }

        public Option<IExpressionConstant> SuccessResult
        {
            get { return Right; }
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

        public static LiquidExpressionResult Success(Option<IExpressionConstant> success)
        {
            return new LiquidExpressionResult(success);
        }

        public static LiquidExpressionResult Success(IExpressionConstant success)
        {
            return Success(new Some<IExpressionConstant>(success));
        }
    }
}
