using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.ILiquidValue>>;

namespace Liquid.NET.Expressions
{
    public class VariableReference : ExpressionDescription
    {

        public String Name { get; private set; }

        public VariableReference(String name)
        {
            Name = name;
        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
        {
            var lookupResult= templateContext.SymbolTableStack.Reference(Name);
            return lookupResult.IsSuccess ? 
                lookupResult :
                ErrorOrNone(templateContext, lookupResult);
        }

        private LiquidExpressionResult ErrorOrNone(ITemplateContext templateContext, LiquidExpressionResult failureResult)
        {
            if (templateContext.Options.ErrorWhenValueMissing)
            {
                return failureResult;
            }
            else
            {
                return LiquidExpressionResult.Success(new None<ILiquidValue>());
            }
        }
    }
}
