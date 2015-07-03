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
    public class ObjectReferenceChain : ExpressionDescription
    {
        private readonly LiquidExpression _liquidExpression;

        public ObjectReferenceChain(LiquidExpression liquidExpression)
        {
            _liquidExpression = liquidExpression;
        }

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            // something got screwed up here
            return LiquidExpressionEvaluator.Eval(_liquidExpression, new List<Option<IExpressionConstant>>(), templateContext);
        }
    }
}
