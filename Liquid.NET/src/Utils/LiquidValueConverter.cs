using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Liquid.NET.Constants;

namespace Liquid.NET.Utils
{
    public class LiquidValueConverter
    {
        public Option<ILiquidValue> Convert(Object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return Option<ILiquidValue>.None();
            }

            var valueFromPrimitive = FromPrimitive(obj);
            if (!ReferenceEquals(valueFromPrimitive, null))
            {
                return valueFromPrimitive;
            }

            var valueFromObject = FromObject(obj);
            if (!ReferenceEquals(valueFromObject, null))
            {
                return valueFromObject;
            }

            throw new Exception("can't convert object of type" + obj.GetType());
        }

        private Option<ILiquidValue> FromObject(Object obj)
        {
            var newHash = new LiquidHash();
            var kvps = obj.GetType()
                .GetProperties()
                .Where(property => !(property.GetCustomAttributes<LiquidIgnoreAttribute>().Any() 
                     || (property.GetCustomAttributes<LiquidIgnoreIfNullAttribute>().Any() 
                        && ReferenceEquals(property.GetGetMethod().Invoke(obj, null), null))))
                .Select(property => new KeyValuePair<String, Option<ILiquidValue>> (
                    GetPropertyName(property),
                    GetPropertyValue(obj, property)));

            foreach (var kvp in kvps)
            {
                // tolower may create a collision
                if (newHash.ContainsKey(kvp.Key))
                {
                    newHash[kvp.Key] = kvp.Value;
                }
                else
                {
                    newHash.Add(kvp);
                }
            }
            return newHash;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            var overriddenName = property.GetCustomAttributes<LiquidNameAttribute>().FirstOrDefault();

            return overriddenName == null ? property.Name.ToLowerInvariant() : overriddenName.Key.ToLowerInvariant();
            
        }

        private Option<ILiquidValue> GetPropertyValue(Object obj, PropertyInfo property)
        {
            var val = property.GetGetMethod().Invoke(obj, null);

            return ReferenceEquals(val, null)
                        ? Option<ILiquidValue>.None()
                        : Convert(val);
        }

        private Option<ILiquidValue> FromPrimitive(Object obj)
        {
            if (obj is bool)
            {
                return new LiquidBoolean((bool)obj);
            }

            if (IsInt32Like(obj))
            {
                var val = System.Convert.ToInt32(obj);
                return LiquidNumeric.Create(val);
            }

            if (IsLong(obj))
            {
                var val = System.Convert.ToInt64(obj);
                return LiquidNumeric.Create(val);
            }

            if (obj is DateTime)
            {
                var val = System.Convert.ToDateTime(obj);
                return new LiquidDate(val);
            }

            if (IsDecimalLike(obj))
            {
                var val = System.Convert.ToDecimal(obj);
                return LiquidNumeric.Create(val);
            }

            if (obj is BigInteger)
            {
                var val = (BigInteger)obj;
                return LiquidNumeric.Create(val);
            }

            var str = obj as String;
            if (str != null)
            {
                return LiquidString.Create(str);
            }

            if (IsList(obj))
            {
                var val = obj as IList;
                return CreateCollection(val);
            }

            if (IsDictionary(obj))
            {
                var val = obj as IDictionary;
                return CreateHash(val);
            }

            if (IsGuid(obj))
            {
                var val = obj as Guid?;
                if (val.HasValue)
                {
                    return LiquidString.Create(val.Value.ToString("D"));
                }
            }
            return null;
        }

        private Option<ILiquidValue> CreateHash(IDictionary dict)
        {
            var newHash = new LiquidHash();
            foreach (var key in dict.Keys)
            {
                String keyAsStr = key.ToString();
                var newValue = Convert(dict[key]); // may throw exception
                if (newHash.ContainsKey(keyAsStr))
                {
                    newHash[keyAsStr] = newValue;
                }
                else
                {
                    newHash.Add(keyAsStr,newValue);
                }
            }
            return newHash;
        }

        private Option<ILiquidValue> CreateCollection(IEnumerable list)
        {
            var coll= new LiquidCollection();
            foreach (var item in list)
            {
                coll.Add(Convert(item)); // may throw exception
            }
            return coll;
        }

        private Boolean IsDictionary(Object value)
        {
            return value is IDictionary;
        }

        private bool IsList(Object value)
        {
            return value is IList;
        }

        private bool IsGuid(Object value)
        {
            return value is Guid;
        }
        // http://stackoverflow.com/questions/1130698/checking-if-an-object-is-a-number-in-c-sharp#answer-1130705
        private bool IsInt32Like(object value)
        {
            return value is SByte
                   || value is Byte
                   || value is Int16
                   || value is UInt16 // ignore the fact that this is unsigned
                   || value is Int32
                   || value is UInt32; // ignore the fact that this is unsigned
        }

        private bool IsLong(object value)
        {
            return value is Int64 || value is UInt64;
        }

        private bool IsDecimalLike(object value)
        {
            return value is Single || value is Double || value is Decimal;
        }
       
    }

}
