using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

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

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
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
