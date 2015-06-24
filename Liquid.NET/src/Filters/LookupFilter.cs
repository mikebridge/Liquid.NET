using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{

    public class LookupFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly ExpressionConstant _propertyName;

        public LookupFilter(ExpressionConstant propertyName)
        {
            _propertyName = propertyName;
        }

//        public override IExpressionConstant Apply(ExpressionConstant liquidExpression)
//        {
//            //Console.WriteLine("APPLYING LOOKUP ");
//            return ApplyTo((dynamic) liquidExpression);
//        }
        public override LiquidExpressionResult ApplyTo(IExpressionConstant liquidExpression)
        {
            //return base.ApplyTo(liquidExpression);
            //Console.WriteLine("  ()() TRIED TO DEREFERENCE  " + _propertyName.Value.ToString());
            return LiquidExpressionResult.Error("Unable to dereference " + liquidExpression.Value + " with " + _propertyName.Value + ": expected Array or Dictionary.");
            //return liquidExpression;

            //return new Undefined(_propertyName.Value.ToString());
        }


        public override LiquidExpressionResult ApplyTo(ArrayValue arrayValue)
        {

            String propertyNameString = ValueCaster.RenderAsString(_propertyName);
            int index;
            if (propertyNameString.ToLower().Equals("first"))
            {
                index = 0;
            }
            else if (propertyNameString.ToLower().Equals("last"))
            {
                index = arrayValue.ArrValue.Count - 1;
            }
            else if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(arrayValue.ArrValue.Count));
            }
            else
            {
                var maybeIndex = ValueCaster.Cast<IExpressionConstant, NumericValue>(_propertyName);
                if (!maybeIndex.IsUndefined)
                {
                    index = maybeIndex.IntValue;
                }
                else
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
            }

            if (arrayValue.ArrValue.Count == 0)
            {
                return LiquidExpressionResult.Error("array is empty: " + propertyNameString);
            }
            var result = arrayValue.ValueAt(index); 
            return LiquidExpressionResult.Success(result);
        }

        public override LiquidExpressionResult ApplyTo(DictionaryValue dictionaryValue)
        {
            
            String propertyNameString = ValueCaster.RenderAsString(_propertyName);
            if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(dictionaryValue.DictValue.Keys.Count()));
            }

            return LiquidExpressionResult.Success(dictionaryValue.ValueAt(_propertyName.Value.ToString()));
        }

        // TODO: this is inefficient and ugly and duplicates much of ArrayValue
        public override LiquidExpressionResult ApplyTo(StringValue strValue)
        {
            var strValues = strValue.StringVal.ToCharArray().Select(ch => (IExpressionConstant) new StringValue(ch.ToString())).ToList();
            String propertyNameString = ValueCaster.RenderAsString(_propertyName);
            int index;
            if (propertyNameString.ToLower().Equals("first"))
            {
                index = 0;
            }
            else if (propertyNameString.ToLower().Equals("last"))
            {
                index = strValues.Count - 1;
            }
            else if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(strValues.Count));
            }
            else
            {
                var maybeIndex = ValueCaster.Cast<IExpressionConstant, NumericValue>(_propertyName);
                if (!maybeIndex.IsUndefined)
                {
                    index = maybeIndex.IntValue;
                }
                else
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
            }

            if (strValues.Count == 0)
            {
                return LiquidExpressionResult.Error("Empty string: " + propertyNameString);
            }
            return LiquidExpressionResult.Success(ArrayIndexer.ValueAt(strValues, index));

        }


    }
}
