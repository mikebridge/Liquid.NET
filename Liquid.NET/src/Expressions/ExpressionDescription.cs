using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public abstract class ExpressionDescription : IExpressionDescription
    {
        public virtual void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            throw new NotImplementedException();
        }

        public abstract IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions);

        public bool HasError
        {
            get { return ErrorMessage != null; }
        }

        public string ErrorMessage { get; set; }

        public bool HasWarning
        {
            get { return WarningMessage != null; }
        }


        public string WarningMessage { get; set; }
//
//        public IExpressionDescription Bind(Func<IExpressionDescription, IExpressionDescription> f)
//        {
//            return HasError ? this : f(this);
//        }
        
    }
}
