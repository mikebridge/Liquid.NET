using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    // TODO: determine whether the keys should be case insensitive or not.
    public class DictionaryValue : ExpressionConstant
    {
        private readonly IDictionary<String, Option<IExpressionConstant>> _value;

        public DictionaryValue(IDictionary<String, Option<IExpressionConstant>> dictionary)
        {
            _value = dictionary;
        }

        public DictionaryValue(IDictionary<String, IExpressionConstant> dictionary)
        {
            _value = dictionary.ToDictionary(kv => kv.Key, v => v.Value == null? Option<IExpressionConstant>.None() : v.Value.ToOption());
        }

        public override object Value
        {
            get { return _value; }
        }

        public IDictionary<String, Option<IExpressionConstant>> DictValue { get { return _value; } }

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

        public bool ContainsKey(string key)
        {
            return _value.ContainsKey(key);
        }

        public override string ToString()
        {
            return "{ " +
                   String.Join(", ", DictValue
                       .Keys
                       .Select(key => FormatKvPair(key, DictValue[key]))
                       ) + " }";
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