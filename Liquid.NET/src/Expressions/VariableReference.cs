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
            return lookupResult.IsSuccess 
                ? (lookupResult.SuccessResult.HasValue ? lookupResult : LiquidExpressionResult.ErrorOrNone(templateContext, Name)) 
                : LiquidExpressionResult.MissingOrNone(templateContext, Name);
        }
    }
}
