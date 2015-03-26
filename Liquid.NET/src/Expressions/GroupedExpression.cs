using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class GroupedExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {            
            var childExpressions = expressions.ToList();
            Console.WriteLine("Evaluating children ");
            return childExpressions.Count != 1 ? ConstantFactory.CreateError<BooleanValue>("Unable to parse expression in parentheses") : childExpressions.First();
        }
    }
}
