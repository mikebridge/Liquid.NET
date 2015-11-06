using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{

    public class LiquidCollection : LiquidValue, IList<Option<ILiquidValue>>
    {
        private readonly IList<Option<ILiquidValue>> _values;

        public LiquidCollection()
        {
            _values = new List<Option<ILiquidValue>>();
        }

        public LiquidCollection(IList<Option<ILiquidValue>> values)
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

        //public IList<Option<ILiquidValue>> ArrValue { get { return _values; } }

        public Option<ILiquidValue> ValueAt(int key)
        {
            return CollectionIndexer.ValueAt(_values, key);
        }

        IEnumerator<Option<ILiquidValue>> IEnumerable<Option<ILiquidValue>>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public void Add(Option<ILiquidValue> item)
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

        public bool Contains(Option<ILiquidValue> item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(Option<ILiquidValue>[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Option<ILiquidValue> item)
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
            //var strs = liquidCollection.ArrValue.Select(x => Quote(GetWrappedType(x), RenderAsString(x)));
            //return "[ " + String.Join(", ", strs) + " ]"; 

            // The Concatenated way:
            var strs = _values.Where(x => x.HasValue).Select(x => x.Value.ToString());
            return String.Join("", strs); 
        }

        public int IndexOf(Option<ILiquidValue> item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, Option<ILiquidValue> item)
        {
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values.RemoveAt(index);
        }


        public Option<ILiquidValue> this[int key]
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
