using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescription
    {
        void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor);

        LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> childresults);

//        bool HasError { get; }
//
//        String ErrorMessage { get; set; }
//
//        bool HasWarning { get; }
//
//        String WarningMessage { get; set; }

    }
}
