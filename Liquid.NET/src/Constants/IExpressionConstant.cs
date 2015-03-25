using System;
using Liquid.NET.Expressions;

namespace Liquid.NET.Constants
{
    public interface IExpressionConstant : IExpressionDescription
    {
        Object Value { get; }

        bool IsTrue { get; }

        IExpressionConstant Bind(Func<IExpressionConstant, IExpressionConstant> f);
    }
}
