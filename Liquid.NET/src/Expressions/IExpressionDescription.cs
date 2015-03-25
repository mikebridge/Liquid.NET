using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescription
    {
        void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor);

        IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> childresults);

        bool HasError { get; }

        String ErrorMessage { get; set; }

        bool HasWarning { get; }

        String WarningMessage { get; set; }

    }
}
