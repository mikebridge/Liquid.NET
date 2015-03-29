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
        /// <returns></returns>
        public override IExpressionConstant Apply(ExpressionConstant liquidExpression)
        {
            return ApplyTo((dynamic)liquidExpression);


        }

        public override IExpressionConstant ApplyTo(IExpressionConstant liquidExpression)
        {
            return ConstantFactory.CreateError<ArrayValue>("Can't ask for an element at that index.  This is not an array or a string.");

        }

        public IExpressionConstant ApplyTo(ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(liquidArrayExpression.ArrValue.Count - 1));
            return positionFilter.ApplyTo(liquidArrayExpression);
        }

        public IExpressionConstant ApplyTo(StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || String.IsNullOrEmpty(liquidStringExpression.StringVal))
            {
                return ConstantFactory.CreateError<StringValue>("String is nil");
            }
            var positionFilter = new PositionFilter(new NumericValue(liquidStringExpression.StringVal.Length - 1));
            return positionFilter.ApplyTo(liquidStringExpression);
        }
    }
}
