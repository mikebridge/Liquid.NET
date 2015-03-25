using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class LastFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {

        public override IExpressionConstant Apply(ExpressionConstant objectExpression)
        {
            return ApplyTo((dynamic)objectExpression);


        }

        public IExpressionConstant ApplyTo(IExpressionConstant objectExpression)
        {
            return ExpressionConstant.CreateError<ArrayValue>("Can't ask for an element at that index.  This is not an array or a string.");

        }

        public IExpressionConstant ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return ExpressionConstant.CreateError<ArrayValue>("Array is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(objectExpression.ArrValue.Count - 1));
            return positionFilter.ApplyTo(objectExpression);
        }

        public IExpressionConstant ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || String.IsNullOrEmpty(objectExpression.StringVal))
            {
                return ExpressionConstant.CreateError<StringValue>("String is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(objectExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(objectExpression);
        }
    }
}
