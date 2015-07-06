using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class FirstFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly PositionFilter _positionFilter;

        public FirstFilter()
        {
            _positionFilter = new PositionFilter(new NumericValue(0));
        }

        public override LiquidExpressionResult Apply(ITemplateContext ctx, ExpressionConstant liquidConstantExpression)
        {
            return _positionFilter.Apply(ctx, liquidConstantExpression);
        }
    }
}
