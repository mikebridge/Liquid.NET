using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class FirstFilter : FilterExpression<LiquidValue, IExpressionConstant>
    {
        private readonly PositionFilter _positionFilter;

        public FirstFilter()
        {
            _positionFilter = new PositionFilter(LiquidNumeric.Create(0));
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, LiquidValue liquidConstantExpression)
        {
            return _positionFilter.Apply(ctx, liquidConstantExpression);
        }
    }
}
