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

        Option<IExpressionConstant> ToOption();

        IDictionary<String, Object> MetaData { get; }

    }
}
