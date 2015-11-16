using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescription
    {
        // TODO: This is in the process of being refactored into the visitor pattern.  It's not there yet.
        //LiquidExpressionResult Accept(ITemplateContext symbolTableStack, IEnumerable<Option<ILiquidValue>> childresults);
        void Accept(ILiquidExpressionVisitor symbolTableStack);

    }
}
