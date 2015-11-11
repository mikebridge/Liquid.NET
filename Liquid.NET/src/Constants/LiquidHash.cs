using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class LiquidHash : LiquidValue, IDictionary<String,Option<ILiquidValue>>
    {
        private readonly IDictionary<String, Option<ILiquidValue>> _value;

        public LiquidHash()
        {
            _value = new Dictionary<String, Option<ILiquidValue>>();
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

        private String KeyStrategy(String rawkey)
        {
            return rawkey.ToLowerInvariant().Trim();
        }

        /// <summary>
        /// This will return None if the key is missing, even if ErrorOnMissing is enabled.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Option<ILiquidValue> ValueAt(String key)
        {
            var fixedKey = KeyStrategy(key);
            return ContainsKey(fixedKey) ? _value[fixedKey] : new None<ILiquidValue>();
        }

        private KeyValuePair<string, Option<ILiquidValue>> KeyStrategyForFixedItem(KeyValuePair<string, Option<ILiquidValue>> item)
        {
            return new KeyValuePair<string, Option<ILiquidValue>>(KeyStrategy(item.Key), item.Value);
        }

        public void Add(String key, Option<ILiquidValue> val)
        {
            if (ReferenceEquals(val, null))
            {
                // The implicit conversion from ILiquidValue
                // to Option<ILiquidValue> won't happen if this value is null.
                val = new None<ILiquidValue>();
            }
            _value.Add(KeyStrategy(key), val);
        }

        public bool Remove(string key)
        {
            return _value.Remove(KeyStrategy(key));
        }

        public bool TryGetValue(string key, out Option<ILiquidValue> value)
        {
            return _value.TryGetValue(KeyStrategy(key), out value);
        }

        public Option<ILiquidValue> this[string key]
        {
            get {
                var result = _value[KeyStrategy(key)];
                return result;
            }
            set { _value[ KeyStrategy(key)] = value; }
        }

        public ICollection<string> Keys { get { return _value.Keys; } }

        public ICollection<Option<ILiquidValue>> Values
        {
            get { return _value.Values; }
        }

        public void Add(KeyValuePair<String,Option<ILiquidValue>> kvp)
        {
            var val = kvp.Value;
            if (ReferenceEquals(val, null)) // if the value is null, it may not get implicitly cast.
            {
                //throw new ArgumentException("value must not be null.");
               val = None; 
            }
            _value.Add( KeyStrategy(kvp.Key), val);
        }

        public void Clear()
        {
            _value.Clear();
        }

        public bool Contains(KeyValuePair<string, Option<ILiquidValue>> item)
        {
            return _value.Contains(KeyStrategyForFixedItem(item));
        }


        public void CopyTo(KeyValuePair<string, Option<ILiquidValue>>[] array, int arrayIndex)
        {
            _value.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Option<ILiquidValue>> item)
        {
            return _value.Remove(KeyStrategyForFixedItem(item));
        }

        public int Count { get { return _value.Count;  } }

        public bool IsReadOnly { get { return _value.IsReadOnly; }}

        public bool ContainsKey(string key)
        {
            return _value.ContainsKey( KeyStrategy(key));
        }

        IEnumerator<KeyValuePair<string, Option<ILiquidValue>>> IEnumerable<KeyValuePair<string, Option<ILiquidValue>>>.GetEnumerator()
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


        private String FormatKvPair(string key, Option<ILiquidValue> expressionConstant)
        {
            Type wrappedType = GetWrappedType(expressionConstant);
            String exprConstantAsString = expressionConstant.HasValue ? expressionConstant.Value.ToString() : "null";
            return Quote(typeof(LiquidString), KeyStrategy(key)) + " : " + Quote(wrappedType, exprConstantAsString);
        }

        private static Type GetWrappedType<T>(Option<T> expressionConstant)
            where T : ILiquidValue
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
            return typeof (LiquidString).IsAssignableFrom(origType) || typeof (LiquidDate).IsAssignableFrom(origType);
        }
    }
}