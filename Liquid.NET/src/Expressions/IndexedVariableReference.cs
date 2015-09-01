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

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            //_variableReference.Eval(symbolTableStack, new List<IExpressionConstant>())

            var liquidExpressionResult = _variableReference.Eval(templateContext, new List<Option<IExpressionConstant>>());
            if (liquidExpressionResult.IsError)
            {
                return liquidExpressionResult;
            }

            if (!liquidExpressionResult.SuccessResult.HasValue)
            {
                return LiquidExpressionResult.Success(Option<IExpressionConstant>.None()); ; // no dictionary to look up in
            }
            var dict = liquidExpressionResult.SuccessResult.Value as DictionaryValue;

            if (dict != null)
            {
                return LiquidExpressionResult.Success(dict.ValueAt(Convert.ToString(_index)));
            }

            return LiquidExpressionResult.Success(Option<IExpressionConstant>.None());
            //            // TODO: Implement the rest of this
            //            return new Undefined(_variableReference +"." + _index);
        }

        
    }
}
