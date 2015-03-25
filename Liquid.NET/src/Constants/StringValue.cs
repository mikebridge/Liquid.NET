using System;
using System.Collections;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class StringValue : ExpressionConstant, IEnumerable
    {
        private readonly String _val;

        public override Object Value { get { return _val; } }

        public String StringVal { get { return _val; } }

        public StringValue(String val)
        {
            _val = val;
        }

        /// <summary>
        /// TODO: is an empty string true or false?
        /// </summary>
        public override bool IsTrue
        {
            get { return _val != null; }
        }

        public override void Accept(IExpressionDescriptionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> childresults)
        {
            return this;
        }


        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
