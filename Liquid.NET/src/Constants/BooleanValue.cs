using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class BooleanValue : ExpressionConstant
    {
        private readonly bool _val;

        public BooleanValue(bool val)
        {
            _val = val;       
        }

        public override void Accept(IExpressionDescriptionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IExpressionConstant Eval(
            SymbolTableStack symbolTableStack, 
            IEnumerable<IExpressionConstant> childresults)
        {
            return this;
        }


        public override Object Value { get { return _val; } }

        public override bool IsTrue { get { return _val;  } }

        public bool BoolValue { get { return _val; } }

    }
}