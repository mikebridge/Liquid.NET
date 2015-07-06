using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class PositionFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly NumericValue _index;

        public PositionFilter(NumericValue index)
        {
            // TODO: Document what happens if htere's no arg passed.  Is this null?  What do we do?
            _index = index;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Error("Can't find sub-elements from that object.  It is not an array or a string.");
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            Console.WriteLine("Array is " + liquidArrayExpression.ArrValue.Count);
            Console.WriteLine("Index is " + _index.IntValue);
            
            if (liquidArrayExpression.ArrValue.Count > 0 && liquidArrayExpression.ArrValue.Count >= _index.IntValue + 1)
            {
                return LiquidExpressionResult.Success(liquidArrayExpression.ValueAt(_index.IntValue));
            }
            else
            {
                return LiquidExpressionResult.Error("Array has no element at position " + _index.IntValue);            
            }
//
//            return LiquidExpressionResult.Success(liquidArrayExpression.ArrValue.Count > 0 && 
//                liquidArrayExpression.ArrValue.Count >= _index.IntValue + 1 ? 
//                liquidArrayExpression.ValueAt(_index.IntValue) : 
//                ConstantFactory.CreateError<ArrayValue>("Array has no element at position " + _index.IntValue));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue liquidStringExpression)
        {
            if (liquidStringExpression == null || liquidStringExpression.Value == null)
            {
                return LiquidExpressionResult.Error("String is nil");
            }

            if (liquidStringExpression.StringVal.Length >= _index.IntValue + 1)
            {
               return LiquidExpressionResult.Success(new StringValue(liquidStringExpression.StringVal[_index.IntValue].ToString()));
            }
            else
            {
                return LiquidExpressionResult.Error("String has no element at position " + _index.IntValue);                
            }

//            return LiquidExpressionResult.Success(liquidStringExpression.StringVal.Length >= _index.IntValue + 1
//                ? (IExpressionConstant)new StringValue(liquidStringExpression.StringVal[_index.IntValue].ToString())
//                : ConstantFactory.CreateError<ArrayValue>("String has no element at position " + _index.IntValue));
        }
    }
}
