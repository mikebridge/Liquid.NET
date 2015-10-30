using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DebugFilter : FilterExpression<ExpressionConstant, DictionaryValue>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant expr)
        {

            var metaData = new DictionaryValue(expr.MetaData.ToDictionary(
                k => k.Key,
                v => v.Value == null
                    ? (IExpressionConstant) new StringValue("")
                    : (IExpressionConstant) new StringValue(v.Value.ToString())));

            var result = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"metadata", metaData},
                    {"value", expr},
                    {"type", new StringValue(expr.LiquidTypeName)}
                });
        
            return LiquidExpressionResult.Success(result);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(new StringValue("No Debugging Data for nil"));
        }
    }
}
