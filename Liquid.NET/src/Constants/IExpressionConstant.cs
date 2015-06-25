using System;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;


namespace Liquid.NET.Constants
{
    public interface IExpressionConstant : IExpressionDescription
    {
        Object Value { get; }

        bool IsTrue { get; }

        //bool IsNil { get; set; }

        LiquidExpressionResult Bind(Func<IExpressionConstant, LiquidExpressionResult> f);

        TOut Bind<TOut>(Func<LiquidExpressionResult, TOut> f)
            where TOut : LiquidExpressionResult;

        T ValueAs<T>();

        Option<IExpressionConstant> ToOption();
    }
}
