using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Expressions
{
    public class ContainsExpression :ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            
            IList<IExpressionConstant> exprList = expressions.ToList();
            if (exprList.Count() != 2)
            {
                return ConstantFactory.CreateError<BooleanValue>("Contains is a binary expression but received " + exprList.Count() + "."); 
            }
            if (exprList[0].IsUndefined || exprList[1].IsUndefined)
            {
                Console.WriteLine("******** UNDEFINED");
                return new BooleanValue(false);
            }

            return Contains((dynamic) exprList[0], exprList[1]);
        }

        private IExpressionConstant Contains(IExpressionConstant expr, IExpressionConstant expressionConstant)
        {
            Console.WriteLine("ERROR");
            return ConstantFactory.CreateError<BooleanValue>("Unable to use contains on this type."); 
        }

        private IExpressionConstant Contains(StringValue stringValue, IExpressionConstant expressionConstant)
        {
            String s = ValueCaster.RenderAsString(expressionConstant);
            return stringValue.StringVal.Contains(s) ? new BooleanValue(true) : new BooleanValue(false);
        }

        private IExpressionConstant Contains(ArrayValue arrayValue, IExpressionConstant expressionConstant)
        {
            return new BooleanValue(arrayValue.ArrValue.Any(x => x.Value.Equals(expressionConstant.Value)));
        }

        private IExpressionConstant Contains(DictionaryValue dictValue, IExpressionConstant expressionConstant)
        {
            return new BooleanValue(dictValue.DictValue.Keys.Any(x => x.Equals(expressionConstant.Value)));
        }

    }
}
