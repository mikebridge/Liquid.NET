using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using System.Runtime.Remoting.Messaging;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DebugFilter : FilterExpression<LiquidValue, LiquidHash>
    {
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue expr)
        {

            var metaData = new LiquidHash();
            foreach (var kvp in expr.MetaData)
            {
                metaData.Add(kvp.Key, kvp.Value == null
                    ? Option<ILiquidValue>.Create(LiquidString.Create(""))
                    : Option<ILiquidValue>.Create(LiquidString.Create(kvp.Value.ToString())));
            }

            var result = new LiquidHash
            {
                {"metadata", metaData},
                {"value", Option<ILiquidValue>.Create(expr)},
                {"type", LiquidString.Create(expr.LiquidTypeName)}
            };
           
            return LiquidExpressionResult.Success(result);
        }

        public override LiquidExpressionResult ApplyToNil(ITemplateContext ctx)
        {
            return LiquidExpressionResult.Success(LiquidString.Create("No Debugging Data for nil"));
        }
    }
}
