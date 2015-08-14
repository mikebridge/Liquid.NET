using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Expressions
{
    /// <summary>
    /// a test for blank will return true if one of the following is true:
    /// 1) the value is of any type, and is nil OR
    /// 2) The value is a string and contains only whitespace OR
    /// 3) THe value is an array of zero length OR
    /// 4) THe value is a dictionary with no keys.
    /// </summary>
    public class IsBlankExpression : ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
           
            var list = expressions.ToList();
            if (!list.Any())
            {
                return LiquidExpressionResult.Success(new BooleanValue(true));
            }
            if (list.Count() != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"blank\"");
            }
           
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new BooleanValue(true)); // null is blank.
            }
            return LiquidExpressionResult.Success(new BooleanValue(BlankChecker.IsBlank(list[0].Value)));
        }

         

    }
}
