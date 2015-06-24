using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public abstract class ExpressionDescription : IExpressionDescription
    {
        public virtual void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            throw new NotImplementedException();
        }

        // TODO: Move this out of here.  This should be on the Evaluator class, not in the AST.
        public abstract LiquidExpressionResult Eval(
            SymbolTableStack symbolTableStack,
            IEnumerable<Option<IExpressionConstant>> expressions);

        // TODO: remove this
        public bool HasError
        {
            get { return ErrorMessage != null; }
        }

        // TODO: remove this
        public string ErrorMessage { get; set; }

        // TODO: remove this
        public bool HasWarning
        {
            get { return WarningMessage != null; }
        }

        // TODO: remove this
        public string WarningMessage { get; set; }

    }
}
