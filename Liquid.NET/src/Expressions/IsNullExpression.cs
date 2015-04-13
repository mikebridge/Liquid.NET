using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class IsNullExpression : ExpressionDescription
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
                throw new Exception("Expected one variable to compare with \"null\""); // this will be obsolete when the lexer/parser is split
            }
            if (list[0].IsUndefined)
            {
                return new BooleanValue(false);
            }
            return new BooleanValue(list[0].Value == null);

            //throw new NotImplementedException();
        }
    }
}
