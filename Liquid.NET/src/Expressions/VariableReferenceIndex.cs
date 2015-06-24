using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;


namespace Liquid.NET.Expressions
{
    public class VariableReferenceIndex : ExpressionDescription
    {
        private readonly VariableReference _variableReference;
        private readonly object _index;

        public VariableReferenceIndex(VariableReference variableReference, Object index)
        {
            _variableReference = variableReference;
            _index = index;
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            //_variableReference.Eval(symbolTableStack, new List<IExpressionConstant>())

            DictionaryValue dict = _variableReference.Eval(symbolTableStack, new List<IExpressionConstant>()) as DictionaryValue;
            if (dict != null)
            {
                return dict.ValueAt(Convert.ToString(_index));
            }
            // TODO: Implement the rest of this
            return new Undefined(_variableReference +"." + _index);
        }

        
    }
}
