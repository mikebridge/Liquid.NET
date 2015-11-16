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

        public override LiquidExpressionResult Accept(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
        {
            return LiquidExpressionVisitor.Visit(this, templateContext);
        }
    }
}
