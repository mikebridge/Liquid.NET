using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    // TODO: Is there any point to having an abstract class any more??
    public abstract class ExpressionDescription : IExpressionDescription
    {
//        public virtual void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            throw new NotImplementedException();
//        }

//        public abstract LiquidExpressionResult Accept(
//            ITemplateContext templateContext,
//            IEnumerable<Option<ILiquidValue>> expressions);

        public abstract void Accept(ILiquidExpressionVisitor symbolTableStack);
    }
}
