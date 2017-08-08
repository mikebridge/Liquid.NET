using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    public class UniqFilter : FilterExpression<LiquidCollection, LiquidCollection>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
        {
            LiquidCollection liquidCollection = new LiquidCollection(liquidArrayExpression.Distinct(new EasyOptionComparer()).ToList());
            return LiquidExpressionResult.Success(liquidCollection);
        }
    }
}
