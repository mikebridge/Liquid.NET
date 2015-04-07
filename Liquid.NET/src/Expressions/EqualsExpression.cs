using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class EqualsExpression :ExpressionDescription
    {
        // TODO: These should always return a value, not an evaluatable expression....

        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        //public IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            IList<IExpressionConstant> exprList = expressions.ToList();

            //Console.WriteLine("EqualsExpression is Eval-ing expressions ");
            if (exprList.Count() != 2)
            {
                // This shouldn't happen if the parser is correct.
                return ConstantFactory.CreateError<BooleanValue>("Equals is a binary expression but received " + exprList.Count() + "."); 
            }
            if (exprList[0].IsUndefined && exprList[1].IsUndefined)
            {
                return new BooleanValue(true);
            }
            if (exprList[0].IsUndefined)
            {
                return ConstantFactory.CreateError<BooleanValue>(exprList[0].ErrorMessage);
            }
            if (exprList[1].IsUndefined)
            {
                return ConstantFactory.CreateError<BooleanValue>(exprList[1].ErrorMessage);
            }
            if (exprList[0].GetType() == exprList[1].GetType())
            {
                return  new BooleanValue(exprList[0].Value.Equals(exprList[1].Value));

            }
           

            return ConstantFactory.CreateError<BooleanValue>("\"Equals\" implementation can't cast yet"); 
        }


    }
}
