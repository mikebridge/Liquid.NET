using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class VariableReferenceTree : IExpressionDescription
    {

        public VariableReferenceTree Parent { get; set; }

        public IExpressionDescription Value { get; set; }
        //public VariableReferenceTree Value { get; set; }

        public VariableReferenceTree IndexExpression { get; set; }
        
        public LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
        {
            return PartialEval(templateContext, childresults).LiquidExpressionResult;
        }

        public VariableReferenceTreeEvalResult PartialEval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
        {
            var errorWhenValueMissing = templateContext.Options.ErrorWhenValueMissing;

            var childResultsList = childresults as IList<Option<ILiquidValue>> ?? childresults.ToList();
            var valueResult = Value.Eval(templateContext, childResultsList);
            if (valueResult.IsError)
            {
                return new VariableReferenceTreeEvalResult(valueResult, new None<ILiquidValue>(), new None<ILiquidValue>());
            }
            LiquidExpressionResult indexResult;
            if (IndexExpression != null)
            {
                indexResult = IndexExpression.Eval(templateContext, childResultsList);
                if (indexResult.IsError)
                {
                    //return indexResult;
                    return new VariableReferenceTreeEvalResult(indexResult, valueResult.SuccessResult, new None<ILiquidValue>());
                }
            }
            else
            {

                return new VariableReferenceTreeEvalResult(valueResult, valueResult.SuccessResult, new None<ILiquidValue>());
            }

            if (!valueResult.SuccessResult.HasValue)
            {
                return new VariableReferenceTreeEvalResult(LiquidExpressionResult.Success(new None<ILiquidValue>()), valueResult.SuccessResult, new None<ILiquidValue>());
                //return LiquidExpressionResult.Success(new None<ILiquidValue>());
                //return LiquidExpressionResult.Error(SymbolTable.NotFoundError(valueResult));
            }
            if (!indexResult.SuccessResult.HasValue)
            {
                return new VariableReferenceTreeEvalResult(LiquidExpressionResult.Success(new None<ILiquidValue>()), valueResult.SuccessResult, indexResult.SuccessResult);
                //return LiquidExpressionResult.Success(new None<ILiquidValue>());
                //return LiquidExpressionResult.Error("ERROR: the index for "+valueResult.SuccessResult.Value+" has no value");
            }
            var result = new IndexDereferencer().Lookup(
                valueResult.SuccessResult.Value,
                indexResult.SuccessResult.Value,
                errorWhenValueMissing);
            return new VariableReferenceTreeEvalResult(result, valueResult.SuccessResult, indexResult.SuccessResult);
        }

        public class VariableReferenceTreeEvalResult
        {
            public VariableReferenceTreeEvalResult(
                LiquidExpressionResult result,
                Option<ILiquidValue> lastValue,
                Option<ILiquidValue> index)
            {
                LiquidExpressionResult = result;
                LastValue = lastValue;
                Index = index;
            }

            public LiquidExpressionResult LiquidExpressionResult { get; private set; }
            public Option<ILiquidValue> LastValue { get; private set; }
            public Option<ILiquidValue> Index { get; private set; }
        }

    }
}
