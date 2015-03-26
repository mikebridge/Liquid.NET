using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class NotExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            IList<IExpressionConstant> exprList = expressions.ToList();
            if (exprList.Count() != 1)
            {
                return ConstantFactory.CreateError<BooleanValue>("\"Not\" is a unary expression but received " + exprList.Count() + " arguments.");
            }
            return new BooleanValue(! exprList[0].IsTrue);
        }
    }
}
