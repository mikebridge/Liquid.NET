using System;
using System.Linq;
using Liquid.NET.Constants;

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
        public override IExpressionConstant ApplyTo(IExpressionConstant liquidExpression)
        {
            //return base.ApplyTo(liquidExpression);
            //Console.WriteLine("  ()() TRIED TO DEREFERENCE  " + _propertyName.Value.ToString());
            liquidExpression.ErrorMessage = "Unable to dereference " + liquidExpression.Value + " with " + _propertyName.Value + ": expected Array or Dictionary.";
            return liquidExpression;

            //return new Undefined(_propertyName.Value.ToString());
        }


        public override IExpressionConstant ApplyTo(ArrayValue arrayValue)
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
                return new NumericValue(arrayValue.ArrValue.Count);
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
                    return ConstantFactory.CreateNilValueOfType<StringValue>("invalid array index: " + propertyNameString);
                }
            }

            if (arrayValue.ArrValue.Count == 0)
            {
                return ConstantFactory.CreateNilValueOfType<StringValue>("array is empty: " + propertyNameString);
            }
            var result = arrayValue.ValueAt(index); 
            return result;
        }

        public override IExpressionConstant ApplyTo(DictionaryValue dictionaryValue)
        {
            
            String propertyNameString = ValueCaster.RenderAsString(_propertyName);
            if (propertyNameString.ToLower().Equals("size"))
            {
                return new NumericValue(dictionaryValue.DictValue.Keys.Count());
            }

            var result = dictionaryValue.ValueAt(_propertyName.Value.ToString());
            return result;
        }

        // TODO: this is inefficient and ugly and duplicates much of ArrayValue
        public override IExpressionConstant ApplyTo(StringValue strValue)
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
                return new NumericValue(strValues.Count);
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
                    return ConstantFactory.CreateNilValueOfType<StringValue>("invalid array index: " + propertyNameString);
                }
            }

            if (strValues.Count == 0)
            {
                return ConstantFactory.CreateNilValueOfType<StringValue>("Empty string: " + propertyNameString);
            }
            return ArrayIndexer.ValueAt(strValues, index);

        }


    }
}
