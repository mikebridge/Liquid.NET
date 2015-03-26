using System;
using Liquid.NET.Expressions;

namespace Liquid.NET.Constants
{
    public interface IExpressionConstant : IExpressionDescription
    {
        Object Value { get; }

        bool IsTrue { get; }

        bool IsUndefined { get; set; }

        bool IsNil { get; }

        IExpressionConstant Bind(Func<IExpressionConstant, IExpressionConstant> f);

        TOut Bind<TOut>(Func<IExpressionConstant, TOut> f)
            where TOut : IExpressionConstant;
    }
}
