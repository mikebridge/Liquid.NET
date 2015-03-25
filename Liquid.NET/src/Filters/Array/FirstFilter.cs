using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class FirstFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly PositionFilter _positionFilter;

        public FirstFilter()
        {
            _positionFilter = new PositionFilter(new NumericValue(0));
        }
        
        // TODO: What does this do when there are no elements?
        public override IExpressionConstant Apply(ExpressionConstant objectExpression)
        {
            return _positionFilter.Apply(objectExpression);
        }
    }
}
