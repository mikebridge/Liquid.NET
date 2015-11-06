using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    public class IsEmptyExpression : ExpressionDescription
    {
//        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            expressionDescriptionVisitor.Visit(this);
//        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            //Console.WriteLine("** ISEMPTY EXPRESSION");
            var list = expressions.ToList();
            if (list.Count != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"empty\"");
            }
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true)); // nulls are empty.
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(EmptyChecker.IsEmpty(list[0].Value)));
        }
        
    }
}
