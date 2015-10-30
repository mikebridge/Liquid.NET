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

        public T SuccessValue<T>()
            where T: IExpressionConstant
        {
            return (T) Right.Value;
        }

        public Option<T> SuccessOption<T>()
            where T : IExpressionConstant
        {
            //return (Option<T>) ((dynamic) Right);
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

        public static LiquidExpressionResult Success(Option<IExpressionConstant> success)
        {
            return new LiquidExpressionResult(success);
        }

        public static LiquidExpressionResult Success(String successString)
        {
            return Success(new StringValue(successString));
        }

    }

    public static class LiquidExpressionResultExtensions
    {

        public static LiquidExpressionResult Bind<T>(this LiquidExpressionResult self, Func<Option<T>, LiquidExpressionResult> f)
            where T: IExpressionConstant
        {
            if (f == null) throw new ArgumentNullException("f");

            return self.IsError
                ? self
                : f(self.SuccessOption<T>());
                
        }
    }

}
