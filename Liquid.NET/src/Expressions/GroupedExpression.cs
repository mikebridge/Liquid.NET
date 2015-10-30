using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class GroupedExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var childExpressions = expressions.ToList();

            return childExpressions.Count != 1 ? 
                LiquidExpressionResult.Error("Unable to parse expression in parentheses") : 
                LiquidExpressionResult.Success(childExpressions.First());
        }
    }
}
