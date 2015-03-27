using System;
using System.Collections.Generic;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class ObjectValue : ExpressionConstant
    {
        //private readonly Object _val;
        private readonly IExpressionConstant _dictionaryValue;
        private readonly Reflector _reflector = new Reflector();


        public ObjectValue(Object val)
        {
            //_val = val;
            //Console.WriteLine("REFLECTING " + val);
            _dictionaryValue = _reflector.GenerateExpressionConstant(val);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            return this;
        }

        public override object Value { get { return _dictionaryValue; } }

        // TODO: Is this correct?
        public override bool IsTrue
        {
            get { return _dictionaryValue != null; }
        }
    }
}
