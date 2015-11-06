using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class ContainsExpression :ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<IExpressionConstant>> expressions)
        {
            IList<Option<IExpressionConstant>> exprList = expressions.ToList();
            if (exprList.Count != 2)
            {
                return LiquidExpressionResult.Error("Contains is a binary expression but received " + exprList.Count + "."); 
            }
            if (!exprList[0].HasValue || !exprList[1].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }

            //return Contains((dynamic) exprList[0].Value, exprList[1].Value);
            var arr = exprList[0].Value as LiquidCollection;
            if (arr != null)
            {
                return Contains(arr, exprList[1].Value);
            }
            var dict = exprList[0].Value as LiquidHash;
            if (dict != null)
            {
                return Contains(dict, exprList[1].Value);
            }
            var str = exprList[0].Value as LiquidString;
            if (str != null)
            {
                return Contains(str, exprList[1].Value);
            }
            return Contains(exprList[0].Value, exprList[1].Value);
        }

        // ReSharper disable UnusedParameter.Local
        private LiquidExpressionResult Contains(IExpressionConstant expr, IExpressionConstant expressionConstant)
        // ReSharper restore UnusedParameter.Local
        {
            return LiquidExpressionResult.Error("Unable to use 'contains' on this type."); 
        }

        private LiquidExpressionResult Contains(LiquidString liquidString, IExpressionConstant expressionConstant)
        {
            String s = ValueCaster.RenderAsString(expressionConstant);
            return LiquidExpressionResult.Success(liquidString.StringVal.Contains(s) ? new LiquidBoolean(true) : new LiquidBoolean(false));
        }

        private LiquidExpressionResult Contains(LiquidCollection liquidCollection, IExpressionConstant expressionConstant)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(liquidCollection.Any(IsEqual(expressionConstant))));
        }

        private LiquidExpressionResult Contains(LiquidHash dictValue, IExpressionConstant expressionConstant)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(dictValue.Keys.Any(x => x.Equals(expressionConstant.Value))));
        }

        private static Func<Option<IExpressionConstant>, bool> IsEqual(IExpressionConstant expressionConstant)
        {            
            return x =>
            {
                if (x.HasValue)
                {
                    return x.Value.Value.Equals(expressionConstant.Value);
                }
                else
                {
                    return expressionConstant == null;
                }
            };
        }
    }
}
