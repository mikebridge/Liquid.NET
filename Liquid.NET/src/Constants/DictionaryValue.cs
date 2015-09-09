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
            //Console.WriteLine("VALUE AT " + key);
            // TODO: Fix this.
            var result = _value.ContainsKey(key) ? _value[key] : new None<IExpressionConstant>(); // new Undefined(key);
            //var result = _value.ContainsKey(key) ? _value[key] : FilterFactory.CreateUndefinedForType(typeof(StringValue))
            //Console.WriteLine("IS " + result);
            return result;
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
            String exprConstantAsString = expressionConstant.HasValue? expressionConstant.Value.ToString() : "null";
            return Quote(typeof(StringValue), key) + " : " + Quote(wrappedType, exprConstantAsString);
        }

        private static Type GetWrappedType<T>(Option<T> expressionConstant)
            where T : IExpressionConstant
        {
            if (expressionConstant.HasValue)
            {
                var nestedType = expressionConstant.Value.GetType();
                //Console.WriteLine("NEsted type " + nestedType);
                return nestedType;
            }
            else
            {
                return null;
            }
        }

        public static String Quote(Type origType, String str)
        {
            if (origType == null)
            {
                return "null";
            }
            //            if (typeof(NumericValue).IsAssignableFrom(origType) || typeof(BooleanValue).IsAssignableFrom(origType) ||
            //                if (typeof(ArrayValue).IsAssignableFrom(origType) || typeof(DictionaryValue).IsAssignableFrom(origType))
            //            {
            if (typeof(StringValue).IsAssignableFrom(origType))
            {
                return "\"" + str + "\"";
            }
            else
            {
                return str;
            }
        }

    }
}