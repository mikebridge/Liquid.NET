using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class VariableReference : ExpressionDescription
    {

        public String Name { get; private set; }

        public VariableReference(String name)
        {
            Name = name;
        }

        public override void Accept(IExpressionDescriptionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> childresults)
        {
            return symbolTableStack.Reference(Name);
        }


    }
}
