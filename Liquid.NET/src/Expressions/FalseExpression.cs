using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class FalseExpression : ExpressionDescription
    {
        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            return LiquidExpressionResult.Success(new Some<IExpressionConstant>(new BooleanValue(false)));
        }
    }
}
