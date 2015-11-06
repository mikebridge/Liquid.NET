using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class OrExpression : ExpressionDescription
    {
//        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            expressionDescriptionVisitor.Visit(this);
//        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            var exprList = expressions.ToList();
            return LiquidExpressionResult.Success(new LiquidBoolean(exprList.Any(x => x.HasValue && x.Value.IsTrue)));
        }
    }
}