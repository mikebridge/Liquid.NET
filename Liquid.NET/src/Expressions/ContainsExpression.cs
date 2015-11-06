using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class ContainsExpression :ExpressionDescription
    {

        public override LiquidExpressionResult Eval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
        {
            IList<Option<ILiquidValue>> exprList = expressions.ToList();
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
        private LiquidExpressionResult Contains(ILiquidValue expr, ILiquidValue liquidValue)
        // ReSharper restore UnusedParameter.Local
        {
            return LiquidExpressionResult.Error("Unable to use 'contains' on this type."); 
        }

        private LiquidExpressionResult Contains(LiquidString liquidString, ILiquidValue liquidValue)
        {
            String s = ValueCaster.RenderAsString(liquidValue);
            return LiquidExpressionResult.Success(liquidString.StringVal.Contains(s) ? new LiquidBoolean(true) : new LiquidBoolean(false));
        }

        private LiquidExpressionResult Contains(LiquidCollection liquidCollection, ILiquidValue liquidValue)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(liquidCollection.Any(IsEqual(liquidValue))));
        }

        private LiquidExpressionResult Contains(LiquidHash dictValue, ILiquidValue liquidValue)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(dictValue.Keys.Any(x => x.Equals(liquidValue.Value))));
        }

        private static Func<Option<ILiquidValue>, bool> IsEqual(ILiquidValue liquidValue)
        {            
            return x =>
            {
                if (x.HasValue)
                {
                    return x.Value.Value.Equals(liquidValue.Value);
                }
                else
                {
                    return liquidValue == null;
                }
            };
        }
    }
}
