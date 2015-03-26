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

            Console.WriteLine("EqualsExpression is Eval-ing expressions ");
            if (exprList.Count() != 2)
            {
                // This shouldn't happen if the parser is correct.
                //return new ExpressionError("Equals is a binary expression but received " + exprList.Count() + "."); 
                return ConstantFactory.CreateError<BooleanValue>("Equals is a binary expression but received " + exprList.Count() + "."); 
            }
            if (exprList[0].GetType() == exprList[1].GetType())
            {
                Console.WriteLine("Comparing "+exprList[0].Value +" to "+exprList[1].Value);
                Console.WriteLine(" comparison result " + exprList[0].Value.Equals(exprList[1].Value));
                return  new BooleanValue(exprList[0].Value.Equals(exprList[1].Value));
                //Console.WriteLine("RESULT OF EQUALS: "+result.Value);

            }
            Console.WriteLine("COmparing " + exprList[0].GetType() +" TO " + exprList[1].GetType());

            return ConstantFactory.CreateError<BooleanValue>("\"Equals\" implementation can't cast yet"); 
        }


    }
}
