using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class IsPresentExpression : ExpressionDescription
    {
//        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            expressionDescriptionVisitor.Visit(this);
//        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var list = expressions.ToList();
            if (list.Count != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"present\"");
            }
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false)); // null is not present.
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(!BlankChecker.IsBlank(list[0].Value)));
        }
    }
}
