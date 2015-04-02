using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class ObjectReferenceChain : ExpressionDescription
    {
        private readonly LiquidExpression _liquidExpression;

        public ObjectReferenceChain(LiquidExpression liquidExpression)
        {
            _liquidExpression = liquidExpression;
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            return LiquidExpressionEvaluator.Eval(_liquidExpression, new List<IExpressionConstant>(), symbolTableStack);
        }
    }
}
