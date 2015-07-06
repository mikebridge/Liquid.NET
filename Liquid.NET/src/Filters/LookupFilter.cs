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
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            //return base.ApplyTo(liquidExpression);
            //Console.WriteLine("  ()() TRIED TO DEREFERENCE  " + _propertyName.Value.ToString());
            return LiquidExpressionResult.Error("Unable to dereference " + liquidExpression.Value + " with " + _propertyName.Value + ": expected Array or Dictionary.");



            //return liquidExpression;

            //return new Undefined(_propertyName.Value.ToString());
        }


        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue arrayValue)
        {

            String propertyNameString = ValueCaster.RenderAsString((IExpressionConstant)_propertyName);
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
                var maybeIndexResult = ValueCaster.Cast<IExpressionConstant, NumericValue>(_propertyName);
                if (maybeIndexResult.IsError || !maybeIndexResult.SuccessResult.HasValue)
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
                else
                {
                    index = maybeIndexResult.SuccessValue<NumericValue>().IntValue;
                }
            }

            if (arrayValue.ArrValue.Count == 0)
            {
                //return LiquidExpressionResult.Error("array is empty: " + propertyNameString);
                return LiquidExpressionResult.Success(new None<IExpressionConstant>()); // not an error in Ruby liquid.
            }
            var result = arrayValue.ValueAt(index); 
            return LiquidExpressionResult.Success(result);
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue dictionaryValue)
        {

            String propertyNameString = ValueCaster.RenderAsString((IExpressionConstant)_propertyName);
            if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(dictionaryValue.DictValue.Keys.Count()));
            }

            return LiquidExpressionResult.Success(dictionaryValue.ValueAt(_propertyName.Value.ToString()));
        }

        // TODO: this is inefficient and ugly and duplicates much of ArrayValue
        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, StringValue strValue)
        {
            var strValues = strValue.StringVal.ToCharArray().Select(ch => new StringValue(ch.ToString()).ToOption()).ToList();
            String propertyNameString = ValueCaster.RenderAsString((IExpressionConstant)_propertyName);
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
                var maybeIndexResult = ValueCaster.Cast<IExpressionConstant, NumericValue>(_propertyName);
                if (maybeIndexResult.IsError || !maybeIndexResult.SuccessResult.HasValue)
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
                else
                {
                    index = maybeIndexResult.SuccessValue<NumericValue>().IntValue;
                }
            }

            if (strValues.Count == 0)
            {
                //return LiquidExpressionResult.Error("Empty string: " + propertyNameString);
                return LiquidExpressionResult.Success(new None<IExpressionConstant>()); // not an error in Ruby liquid.
            }
            return LiquidExpressionResult.Success(ArrayIndexer.ValueAt(strValues, index));

        }


    }
}
