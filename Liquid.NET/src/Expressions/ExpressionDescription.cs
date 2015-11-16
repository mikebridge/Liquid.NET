using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public abstract class ExpressionDescription : IExpressionDescription
    {
//        public virtual void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
//        {
//            throw new NotImplementedException();
//        }

        public abstract LiquidExpressionResult Accept(
            ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> expressions);

    }
}
