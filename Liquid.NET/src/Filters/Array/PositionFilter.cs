using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{
    public class PositionFilter : FilterExpression<LiquidValue, ILiquidValue>
    {
        private readonly LiquidNumeric _index;

        public PositionFilter(LiquidNumeric index)
        {
            // TODO: Document what happens if htere's no arg passed.  Is this null?  What do we do?
            _index = index;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ILiquidValue liquidExpression)
        {
            return LiquidExpressionResult.Error("Can't find sub-elements from that object.  It is not an array or a string.");
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
        {
            if (liquidArrayExpression == null || liquidArrayExpression.Value == null)
            {
                return LiquidExpressionResult.Error("Array is nil");
            }
            //Console.WriteLine("Array is " + liquidArrayExpression.ArrValue.Count);
            //Console.WriteLine("Index is " + _index.IntValue);
            
            if (liquidArrayExpression.Count > 0 && liquidArrayExpression.Count >= _index.IntValue + 1)
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
//                ConstantFactory.CreateError<LiquidCollection>("Array has no element at position " + _index.IntValue));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            if (liquidLiquidStringExpression == null || liquidLiquidStringExpression.Value == null)
            {
                return LiquidExpressionResult.Error("String is nil");
            }

            if (liquidLiquidStringExpression.StringVal.Length >= _index.IntValue + 1)
            {
               return LiquidExpressionResult.Success(LiquidString.Create(liquidLiquidStringExpression.StringVal[_index.IntValue].ToString()));
            }
            else
            {
                return LiquidExpressionResult.Error("String has no element at position " + _index.IntValue);                
            }

//            return LiquidExpressionResult.Success(liquidLiquidStringExpression.StringVal.Length >= _index.IntValue + 1
//                ? (ILiquidValue)LiquidString.Create(liquidLiquidStringExpression.StringVal[_index.IntValue].ToString())
//                : ConstantFactory.CreateError<LiquidCollection>("String has no element at position " + _index.IntValue));
        }
    }
}
