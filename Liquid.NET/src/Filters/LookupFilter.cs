using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters
{
    /// <summary>
    /// TODO: Update to new structure
    /// </summary>

    public class LookupFilter : FilterExpression<ExpressionConstant, IExpressionConstant>
    {
        private readonly ExpressionConstant _propertyName;

        public LookupFilter(ExpressionConstant propertyName)
        {
            _propertyName = propertyName;
        }

        public override IExpressionConstant Apply(ExpressionConstant objectExpression)
        {
            //Console.WriteLine("APPLYING LOOKUP ");
            return ApplyTo((dynamic) objectExpression);
        }

        public IExpressionConstant ApplyTo(IExpressionConstant expr)
        {
            //Console.WriteLine("  ()() TRIED TO DEREFERENCE  " + _propertyName.Value.ToString());
            expr.ErrorMessage = "Unable to dereference " + expr.Value + " with "+_propertyName.Value.ToString()+": expected Array or Dictionary.";
            return expr;
            
            //return new Undefined(_propertyName.Value.ToString());
        }

        public IExpressionConstant ApplyTo(ArrayValue arrayValue)
        {
            //Console.WriteLine("Looking up " + _propertyName.Value);
            // I see that arrays can be looked up using "first" or "last".  Let's convert these
            // here to numeric.
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
            else
            {
                var maybeIndex = ValueCaster.Cast<IExpressionConstant, NumericValue>(_propertyName);
                if (!maybeIndex.IsUndefined)
                {
                    index = maybeIndex.IntValue;
                }
                else
                {
                    return new Undefined("invalid array index: "+propertyNameString);
                }
            }
            

            //Console.WriteLine("Looking up index " + index);
            // TODO: Check array bounds 
            var result = arrayValue.ValueAt(index); 
            //Console.WriteLine("RESULT: "+result.Value.ToString());
            return result;
        }

        public IExpressionConstant ApplyTo(DictionaryValue dictionaryValue)
        {
            var result = dictionaryValue.ValueAt(_propertyName.Value.ToString());

            return result;
        }


    }
}
