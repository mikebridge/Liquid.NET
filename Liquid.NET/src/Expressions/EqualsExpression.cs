using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class EqualsExpression :ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            IList<Option<IExpressionConstant>> exprList = expressions.ToList();

            
            if (exprList.Count != 2)
            {
                // This shouldn't happen if the parser is correct.
                return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count + "."); 
            }

            if (!exprList[0].HasValue && !exprList[1].HasValue)
            {
                return LiquidExpressionResult.Success(new BooleanValue(true));
            }
            if (!exprList[0].HasValue || !exprList[1].HasValue)
            {
               return LiquidExpressionResult.Success(new BooleanValue(false));
            }           
            if (exprList[0].GetType() == exprList[1].GetType())
            {
                var isEqual = exprList[0].Value.Equals(exprList[1].Value);
                return  LiquidExpressionResult.Success(new BooleanValue(isEqual));
            }

            return LiquidExpressionResult.Error("\"Equals\" implementation can't cast that yet"); 
        }


    }
}
