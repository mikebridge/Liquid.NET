using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class VariableReferenceTree : IExpressionDescription
    {

        public VariableReferenceTree Parent { get; set; }

        public IExpressionDescription Value { get; set; }

        public IExpressionDescription IndexExpression { get; set; }


        public void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            throw new NotImplementedException();
        }

        public LiquidExpressionResult Eval(ITemplateContext symbolTableStack, IEnumerable<Option<IExpressionConstant>> childresults)
        {
            throw new NotImplementedException();
        }
    }
}
