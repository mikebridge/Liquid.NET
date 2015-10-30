using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescription
    {
        //void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor);

        LiquidExpressionResult Eval(ITemplateContext symbolTableStack, IEnumerable<Option<IExpressionConstant>> childresults);

    }
}
