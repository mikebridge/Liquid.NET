using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;


namespace Liquid.NET.Constants
{
    public interface IExpressionConstant : IExpressionDescription
    {
        Object Value { get; }

        bool IsTrue { get; }

        String LiquidTypeName { get; }

        LiquidExpressionResult Bind(Func<IExpressionConstant, LiquidExpressionResult> f);

        TOut Bind<TOut>(Func<LiquidExpressionResult, TOut> f)
            where TOut : LiquidExpressionResult;

        T ValueAs<T>();

        Option<IExpressionConstant> ToOption();

        IDictionary<String, Object> MetaData { get; }

    }
}
