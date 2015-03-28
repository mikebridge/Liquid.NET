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
        /// <summary>
        ///  TODO: Update to new structure
        /// </summary>
        /// <param name="objectExpression"></param>
        /// <returns></returns>
        public override IExpressionConstant Apply(ExpressionConstant objectExpression)
        {
            return ApplyTo((dynamic)objectExpression);


        }

        public override IExpressionConstant ApplyTo(IExpressionConstant objectExpression)
        {
            return ConstantFactory.CreateError<ArrayValue>("Can't ask for an element at that index.  This is not an array or a string.");

        }

        public IExpressionConstant ApplyTo(ArrayValue objectExpression)
        {
            if (objectExpression == null || objectExpression.Value == null)
            {
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(objectExpression.ArrValue.Count - 1));
            return positionFilter.ApplyTo(objectExpression);
        }

        public IExpressionConstant ApplyTo(StringValue objectExpression)
        {
            if (objectExpression == null || String.IsNullOrEmpty(objectExpression.StringVal))
            {
                return ConstantFactory.CreateError<StringValue>("String is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(objectExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(objectExpression);
        }
    }
}
