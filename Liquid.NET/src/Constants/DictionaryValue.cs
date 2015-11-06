using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class DictionaryValue : ExpressionConstant, IDictionary<String,Option<IExpressionConstant>>
    {
        private readonly IDictionary<String, Option<IExpressionConstant>> _value;

        public DictionaryValue()
        {
            _value = new Dictionary<String, Option<IExpressionConstant>>();
        }

        public override object Value
        {
            get { return _value; }
        }

        public override bool IsTrue
        {
            get { return _value != null;  }
        }

        public override string LiquidTypeName
        {
            get { return "hash"; }
        }

        /// <summary>
        /// This will return None if the key is missing, even if ErrorOnMissing is enabled.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Option<IExpressionConstant> ValueAt(String key)
        {
            return ContainsKey(key) ? _value[key] : new None<IExpressionConstant>();
        }


        public void Add(String key, Option<IExpressionConstant> val)
        {
            if (ReferenceEquals(val, null))
            {
                // The implicit conversion from IExpressionConstant
                // to Option<IExpressionConstant> won't happen if this value is null.
                val = new None<IExpressionConstant>();
            }
            _value.Add(key, val);
        }

        public bool Remove(string key)
        {
            return _value.Remove(key);
        }

        public bool TryGetValue(string key, out Option<IExpressionConstant> value)
        {
            return _value.TryGetValue(key, out value);
        }

        public Option<IExpressionConstant> this[string key]
        {
            get { 
                var result= _value[key];
                return result;
            }
            set { _value[key] = value; }
        }

        public ICollection<string> Keys { get { return _value.Keys; } }

        public ICollection<Option<IExpressionConstant>> Values
        {
            get { return _value.Values; }
        }

        public void Add(KeyValuePair<String,Option<IExpressionConstant>> kvp)
        {
            var val = kvp.Value;
            if (ReferenceEquals(val, null)) // if the value is null, it may not get implicitly cast.
            {
                //throw new ArgumentException("value must not be null.");
               val = Option<IExpressionConstant>.None(); 
            }
            _value.Add(kvp.Key, val);
        }

        public void Clear()
        {
            _value.Clear();
        }

        public bool Contains(KeyValuePair<string, Option<IExpressionConstant>> item)
        {
            return _value.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Option<IExpressionConstant>>[] array, int arrayIndex)
        {
            _value.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Option<IExpressionConstant>> item)
        {
            return _value.Remove(item);
        }

        public int Count { get { return _value.Count;  } }

        public bool IsReadOnly { get { return _value.IsReadOnly; }}

        public bool ContainsKey(string key)
        {
            return _value.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, Option<IExpressionConstant>>> IEnumerable<KeyValuePair<string, Option<IExpressionConstant>>>.GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        public override string ToString()
        {
            return "{ " +
                   String.Join(", ", _value
                       .Keys
                       .Select(key => FormatKvPair(key, _value[key]))
                       ) + " }";
        }

        public IEnumerator GetEnumerator()
        {
            return _value.GetEnumerator();
        }


        private static String FormatKvPair(string key, Option<IExpressionConstant> expressionConstant)
        {
            Type wrappedType = GetWrappedType(expressionConstant);
            String exprConstantAsString = expressionConstant.HasValue ? expressionConstant.Value.ToString() : "null";
            return Quote(typeof(StringValue), key) + " : " + Quote(wrappedType, exprConstantAsString);
        }

        private static Type GetWrappedType<T>(Option<T> expressionConstant)
            where T : IExpressionConstant
        {
            return expressionConstant.HasValue ? expressionConstant.Value.GetType() : null;
        }

        private static String Quote(Type origType, String str)
        {
            if (origType == null)
            {
                return "null";
            }

            return TypeNeedsQuotes(origType)
                ? "\"" + str + "\""
                : str;
        }

        private static bool TypeNeedsQuotes(Type origType)
        {
            return typeof (StringValue).IsAssignableFrom(origType) || typeof (DateValue).IsAssignableFrom(origType);
        }
    }
}