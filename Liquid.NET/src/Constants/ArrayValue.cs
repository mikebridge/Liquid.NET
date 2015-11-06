using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{

    public class ArrayValue : ExpressionConstant, IList<Option<IExpressionConstant>>
    {
        private readonly IList<Option<IExpressionConstant>> _values;

        public ArrayValue()
        {
            _values = new List<Option<IExpressionConstant>>();
        }

        public ArrayValue(IList<Option<IExpressionConstant>> values)
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

        public override string LiquidTypeName
        {
            get { return "collection"; }
        }

        //public IList<Option<IExpressionConstant>> ArrValue { get { return _values; } }

        public Option<IExpressionConstant> ValueAt(int key)
        {
            return ArrayIndexer.ValueAt(_values, key);
        }

        IEnumerator<Option<IExpressionConstant>> IEnumerable<Option<IExpressionConstant>>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public void Add(Option<IExpressionConstant> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(Option<IExpressionConstant> item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(Option<IExpressionConstant>[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Option<IExpressionConstant> item)
        {
            return _values.Remove(item);
        }

        public int Count { get { return _values.Count; } }
        public bool IsReadOnly { get { return _values.IsReadOnly; }}

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public override string ToString()
        {

            // The JSON way:
            //var strs = arrayValue.ArrValue.Select(x => Quote(GetWrappedType(x), RenderAsString(x)));
            //return "[ " + String.Join(", ", strs) + " ]"; 

            // The Concatenated way:
            var strs = _values.Where(x => x.HasValue).Select(x => x.Value.ToString());
            return String.Join("", strs); 
        }

        public int IndexOf(Option<IExpressionConstant> item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, Option<IExpressionConstant> item)
        {
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values.RemoveAt(index);
        }


        public Option<IExpressionConstant> this[int key]
        {
            get
            {
                var result = _values[key];
                return result;
            }
            set { _values[key] = value; }
        }

    }
}
