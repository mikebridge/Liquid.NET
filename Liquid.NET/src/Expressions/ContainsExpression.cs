using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class ContainsExpression :ExpressionDescription
    {

        public override LiquidExpressionResult Accept(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> expressions)
        {
            return LiquidExpressionVisitor.Visit(this, expressions);
        }

        // ReSharper disable UnusedParameter.Local
        public LiquidExpressionResult Contains(ILiquidValue expr, ILiquidValue liquidValue)
        // ReSharper restore UnusedParameter.Local
        {
            return LiquidExpressionResult.Error(String.Format("Unable to use 'contains' on a {0}.", expr.LiquidTypeName)); 
        }

        public LiquidExpressionResult Contains(LiquidString liquidString, ILiquidValue liquidValue)
        {
            String s = ValueCaster.RenderAsString(liquidValue);
            return LiquidExpressionResult.Success(liquidString.StringVal.Contains(s) ? new LiquidBoolean(true) : new LiquidBoolean(false));
        }

        public LiquidExpressionResult Contains(LiquidCollection liquidCollection, ILiquidValue liquidValue)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(liquidCollection.Any(IsEqual(liquidValue))));
        }

        public LiquidExpressionResult Contains(LiquidHash dictValue, ILiquidValue liquidValue)
        {
            return LiquidExpressionResult.Success(new LiquidBoolean(dictValue.Keys.Any(x => x.Equals(liquidValue.Value))));
        }

        public static Func<Option<ILiquidValue>, bool> IsEqual(ILiquidValue liquidValue)
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
