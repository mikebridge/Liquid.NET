using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class ContainsExpression :ExpressionDescription
    {
        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

        public override LiquidExpressionResult Eval(SymbolTableStack symbolTableStack, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            IList<Option<IExpressionConstant>> exprList = expressions.ToList();
            if (exprList.Count() != 2)
            {
                //return LiquidExpressionResult.Error("Contains is a binary expression but received " + exprList.Count() + ".");
                return LiquidExpressionResult.Error("Contains is a binary expression but received " + exprList.Count() + "."); 
            }
            if (!exprList[0].HasValue || !exprList[1].HasValue)
            {
                Console.WriteLine("******** UNDEFINED");
                return LiquidExpressionResult.Success(new BooleanValue(false));
            }

            return Contains((dynamic) exprList[0].Value, exprList[1].Value);
        }

        private LiquidExpressionResult Contains(IExpressionConstant expr, IExpressionConstant expressionConstant)
        {
            Console.WriteLine("ERROR");
            return LiquidExpressionResult.Error("Unable to use 'contains' on this type."); 
        }

        private LiquidExpressionResult Contains(StringValue stringValue, IExpressionConstant expressionConstant)
        {
            String s = ValueCaster.RenderAsString(expressionConstant);
            return LiquidExpressionResult.Success(stringValue.StringVal.Contains(s) ? new BooleanValue(true) : new BooleanValue(false));
        }

        private LiquidExpressionResult Contains(ArrayValue arrayValue, IExpressionConstant expressionConstant)
        {
            return LiquidExpressionResult.Success(new BooleanValue(arrayValue.ArrValue.Any(x => x.Value.Equals(expressionConstant.Value))));
        }

        private LiquidExpressionResult Contains(DictionaryValue dictValue, IExpressionConstant expressionConstant)
        {
            return LiquidExpressionResult.Success(new BooleanValue(dictValue.DictValue.Keys.Any(x => x.Equals(expressionConstant.Value))));
        }

    }
}
