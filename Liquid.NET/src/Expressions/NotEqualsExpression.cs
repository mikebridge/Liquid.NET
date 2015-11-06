using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class NotEqualsExpression :ExpressionDescription
    {
        
//        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            expressionDescriptionVisitor.Visit(this);
//        }

        //public ILiquidValue Eval(SymbolTableStack symbolTableStack, IEnumerable<ILiquidValue> expressions)
        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
        {

            //IList<ILiquidValue> exprList = expressions.ToList();
            IList<Option<ILiquidValue>> exprList = expressions.ToList();

            //Console.WriteLine("EqualsExpression is Eval-ing expressions ");
            if (exprList.Count != 2)
            {
                //return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count() + ".");
                return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count + ".");
            }

            if (exprList.All(x => !x.HasValue)) // both null
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            if (exprList.Any(x => !x.HasValue)) // one null
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true));
            }
           
            if (exprList[0].GetType() == exprList[1].GetType())
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(! exprList[0].Value.Equals(exprList[1].Value)));

            }

            return LiquidExpressionResult.Error("\"Not Equals\" implementation can't cast yet"); 
        }


    }
}
