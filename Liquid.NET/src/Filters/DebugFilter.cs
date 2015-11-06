using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DebugFilter : FilterExpression<ExpressionConstant, DictionaryValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant expr)
        {

            var metaData = new DictionaryValue();
            foreach (var kvp in expr.MetaData)
            {
                metaData.Add(kvp.Key, kvp.Value == null
                    ? Option<IExpressionConstant>.Create(new StringValue(""))
                    : Option<IExpressionConstant>.Create(new StringValue(kvp.Value.ToString())));
            }

            var result = new DictionaryValue
            {
                {"metadata", metaData},
                {"value", Option<IExpressionConstant>.Create(expr)},
                {"type", new StringValue(expr.LiquidTypeName)}
            };
           
            return LiquidExpressionResult.Success(result);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new StringValue("No Debugging Data for nil"));
        }
    }
}
