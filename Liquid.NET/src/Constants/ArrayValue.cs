using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class ArrayValue : ExpressionConstant, IEnumerable<IExpressionConstant>
    {
        private readonly IList<IExpressionConstant> _values;// = new List<IExpressionConstant>(); 

        public ArrayValue(IList<IExpressionConstant> values)
        {
            _values = values;
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

        public IList<IExpressionConstant> ArrValue { get { return _values; } }

        public IExpressionConstant ValueAt(int key)
        {
            return ArrayIndexer.ValueAt(_values, key);
        }

        IEnumerator<IExpressionConstant> IEnumerable<IExpressionConstant>.GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }


        public IEnumerator GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }
    }
}
