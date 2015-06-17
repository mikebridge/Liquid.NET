using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class IsEmptyExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            var list = expressions.ToList();
            if (list.Count() != 1)
            {
                throw new Exception("Expected one variable to compare with \"empty\""); // this will be obsolete when the lexer/parser is split
            }
            return new BooleanValue(EmptyChecker.IsEmpty(list[0]));
        }
    }
}
