using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{

    public class ArrayValue : ExpressionConstant, IEnumerable<Option<IExpressionConstant>>
    {
        private readonly IList<Option<IExpressionConstant>> _values;// = new List<IExpressionConstant>(); 

        public ArrayValue(IList<Option<IExpressionConstant>> values)
        {
            _values = values;
        }

        public ArrayValue(IList<IExpressionConstant> values)
        {
            _values = values.Select(x => x == null? new None<IExpressionConstant>() : x.ToOption()).ToList();
        }

        public override object Value
        {
            get { return _values; }
        }

        // https://docs.shopify.com/themes/liquid-documentation/basics/true-and-false
        public override bool IsTrue
        {
            get { return _values != null; }
        }

        public override string LiquidTypeName
        {
            get { return "collection"; }
        }

        public IList<Option<IExpressionConstant>> ArrValue { get { return _values; } }

        public Option<IExpressionConstant> ValueAt(int key)
        {
            return ArrayIndexer.ValueAt(_values, key);
        }

        IEnumerator<Option<IExpressionConstant>> IEnumerable<Option<IExpressionConstant>>.GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }


        public IEnumerator GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }

        public override string ToString()
        {

            // The JSON way:
            //var strs = arrayValue.ArrValue.Select(x => Quote(GetWrappedType(x), RenderAsString(x)));
            //return "[ " + String.Join(", ", strs) + " ]"; 

            // The Concatenated way:
            var strs = ArrValue.Select(x => x.Value.ToString());
            return String.Join("", strs); 
        }
    }
}
