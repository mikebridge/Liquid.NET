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

        // TODO: not sure what this should do
        public override bool IsTrue
        {
            get { return _value != null;  }
            //get { return _value.Keys.Any(); }
        }

        public override string LiquidTypeName
        {
            get { return "hash"; }
        }

        public Option<IExpressionConstant> ValueAt(String key)
        {
            return _value.ContainsKey(key) ? _value[key] : new None<IExpressionConstant>();
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