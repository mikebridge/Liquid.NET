using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters
{
    /// <summary>
    /// TODO: Find some way to reduce the amout of casting in this class. :(
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
            Console.WriteLine("APPLYING LOOKUP ");
            return ApplyTo((dynamic) objectExpression);
        }

        public IExpressionConstant ApplyTo(IExpressionConstant expr)
        {
            Console.WriteLine("  ()() TRIED TO DEREFERENCE  " + _propertyName.Value.ToString());
            expr.ErrorMessage = "Unable to dereference " + expr.Value + " with "+_propertyName.Value.ToString()+": expected Array or Dictionary.";
            return expr;
            
            //return new Undefined(_propertyName.Value.ToString());
        }

        public IExpressionConstant ApplyTo(ArrayValue arrayValue)
        {
            Console.WriteLine("Looking up index " + Convert.ToInt32(_propertyName.Value));
            var result = arrayValue.ValueAt(Convert.ToInt32(_propertyName.Value)); // yuck!
            Console.WriteLine("RESULT: "+result.Value.ToString());
            return result;
        }

        public IExpressionConstant ApplyTo(DictionaryValue dictionaryValue)
        {
            var result = dictionaryValue.ValueAt(_propertyName.Value.ToString());

            return result;
//            IExpressionConstant result = new Undefined(objectExpression.Value.ToString());
//            if (objectExpression.GetType().IsAssignableFrom(typeof(DictionaryValue)))
//            {
//                Console.WriteLine("*** Looking up " + _propertyName.Value + " IN dictionary");
//                 result = ((DictionaryValue) objectExpression).ValueAt(((StringValue) _propertyName.Value).StringVal);
//            } 
//            else if (objectExpression.GetType().IsAssignableFrom(typeof (ArrayValue)))
//            {
//                Console.WriteLine("*** Looking up " + _propertyName.Value + " IN array");
//                result = ((ArrayValue)objectExpression).ValueAt((int) ((NumericValue) _propertyName.Value).DecimalValue); // yuck!
//  
//            }
//
//           
//            return result;
        }


    }
}
